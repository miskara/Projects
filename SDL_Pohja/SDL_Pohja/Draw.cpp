#include "Draw.h"

Draw* Draw::drawer = NULL;

/*SDL-ikkuna*/
SDL_Window *window;
SDL_GLContext glcontext;

/*??*/
Uint32 start;
SDL_Event event;

/*Teksuurit*/
GLuint texture[1];

/*Shaderijuttuja*/
GLint myLoc;
float shaderZ = 1.0f;

/*Kameran pyöriminen*/
float cameraRotation = 0.0f;
bool cameraRotatingLeft = false;
bool cameraRotatingRight = false;

/*Kameran paikka z-akselilla*/
float camera_Z_postition=-15.0f;

/*Kuvasuhde ja ruudun mitat*/
const float widthOfScreen = 1278.0f;
const float heightOfScreen = 719.0f;
const float aspectratio = widthOfScreen / heightOfScreen;

/*Kuvan päivitystiheys*/
const int FPS = 60;

Draw::Draw(){

	/* For SDL initialization and window creation*/
	SDL_Init(SDL_INIT_VIDEO);			// Initialize SDL2
	window = SDL_CreateWindow(
		"An SDL2 window",				// window title
		SDL_WINDOWPOS_UNDEFINED,		// initial x position
		SDL_WINDOWPOS_UNDEFINED,		// initial y position
		(int)widthOfScreen,							// width, in pixels
		(int)heightOfScreen,							// height, in pixels
		SDL_WINDOW_OPENGL				// flags - see below
		);
	if (window == NULL) {
		fprintf(stderr, "Could not create window: %s\n", SDL_GetError());
		drawer =  NULL;
	}

	/* OpenGL context creation */
	glcontext = SDL_GL_CreateContext(window);
	/*Basic background color and alpha*/
	glClearColor(1.0f, 1.0f, 1.0f, 1.0f);
	/*Initation of Projection Matrix*/
	glMatrixMode(GL_PROJECTION);
	gluPerspective(45, aspectratio, 1.0, 500.0); //Set aspectratio
	glEnable(GL_DEPTH_TEST);
	glShadeModel(GL_SMOOTH);			// Enables Smooth Color Shading

	/*Load textures*/
	if (loadTextures() != true){
		std::cout << "DRAW::init ERROR:Loading textures failed!";
		drawer = NULL;
	}

}

Draw* Draw::init(){
	if (!drawer) {
		drawer = new Draw();
	}
	return drawer;
}

bool Draw::drawBackgroundTexture(){
	/* Enable Texture Mapping */
	glEnable(GL_TEXTURE_2D);
	/* Enable setting texture to background */
	glDepthMask(false);
	
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	/*Binds texture and set polygonmode to FILL*/
	glBindTexture(GL_TEXTURE_2D, texture[0]);
	glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
	glColor3f(1, 1, 1);

	/*Draws giant background image*/
	glBegin(GL_QUADS);
	/*Lasketaan kaikille kuvan nurkille paikat käyttäen kameran etäisyyttä hyödyksi. Ei varmasti oikein mutta testattu toimivaksi.*/
	glTexCoord2f(0.0f, 0.0f); glVertex3f(camera_Z_postition*aspectratio / 2.0f, -camera_Z_postition / 2.0f, camera_Z_postition*1.2f);
	glTexCoord2f(0.0f, 1.0f); glVertex3f(camera_Z_postition*aspectratio / 2.0f, camera_Z_postition / 2.0f, camera_Z_postition*1.2f);
	glTexCoord2f(1.0f, 1.0f); glVertex3f(-camera_Z_postition*aspectratio / 2.0f, camera_Z_postition / 2.0f, camera_Z_postition*1.2f);
	glTexCoord2f(1.0f, 0.0f); glVertex3f(-camera_Z_postition*aspectratio / 2.0f, -camera_Z_postition / 2.0f, camera_Z_postition*1.2f);
	glEnd();

	glDisable(GL_TEXTURE_2D);
	glDepthMask(true);

	return true;
}

bool Draw::loadTextures(){
	SDL_Surface *image1;

	image1 = SDL_LoadBMP("space.bmp");
	if (!image1) {
		SDL_Quit();
		return false;
	}

	// Create Texture	
	glGenTextures(1, &texture[0]);
	glBindTexture(GL_TEXTURE_2D, texture[0]);   // 2d texture (x and y size)

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); // scale linearly when image bigger than texture
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR); // scale linearly when image smalled than texture

	// 2d texture, level of detail 0 (normal), 3 components (red, green, blue), x size from image, y size from image, 
	// border 0 (normal), rgb color data, unsigned byte data, and finally the data itself.
	gluBuild2DMipmaps(GL_TEXTURE_2D, 3, image1->w, image1->h, GL_BGR_EXT, GL_UNSIGNED_BYTE, image1->pixels);

	return true;
}

SDL_Surface *Draw::LoadBMP(char *filename)
{
	Uint8 *rowhi, *rowlo;
	Uint8 *tmpbuf, tmpch;
	SDL_Surface *image;
	int i, j;

	image = SDL_LoadBMP(filename);
	if (image == NULL) {
		fprintf(stderr, "Unable to load %s: %s\n", filename, SDL_GetError());
		return(NULL);
	}

	/* GL surfaces are upsidedown and RGB, not BGR :-) */
	tmpbuf = (Uint8 *)malloc(image->pitch);
	if (tmpbuf == NULL) {
		fprintf(stderr, "Out of memory\n");
		return(NULL);
	}
	rowhi = (Uint8 *)image->pixels;
	rowlo = rowhi + (image->h * image->pitch) - image->pitch;
	for (i = 0; i<image->h / 2; ++i) {
		for (j = 0; j<image->w; ++j) {
			tmpch = rowhi[j * 3];
			rowhi[j * 3] = rowhi[j * 3 + 2];
			rowhi[j * 3 + 2] = tmpch;
			tmpch = rowlo[j * 3];
			rowlo[j * 3] = rowlo[j * 3 + 2];
			rowlo[j * 3 + 2] = tmpch;
		}
		memcpy(tmpbuf, rowhi, image->pitch);
		memcpy(rowhi, rowlo, image->pitch);
		memcpy(rowlo, tmpbuf, image->pitch);
		rowhi += image->pitch;
		rowlo -= image->pitch;
	}
	free(tmpbuf);
	return(image);
}

bool Draw::drawRoom(Room* rom, GLenum mode){
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	//camera position
	glTranslatef(0, 0, camera_Z_postition);
	//camera rotation
	glRotatef(cameraRotation, 0, 1, 0);

	/*Piirretään huone*/
	glPolygonMode(GL_FRONT_AND_BACK, mode);
	glLineWidth(3.0f);
	rom->drawRoom();

	return true;
}

bool Draw::drawSphere(Shape* obj, GLenum mode){
	/*Objektin identity*/
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	//camera position
	glTranslatef(0, 0, camera_Z_postition);
	//camera rotation
	glRotatef(cameraRotation, 0, 1, 0);
	/*Piirretään kappale*/
	glPolygonMode(GL_FRONT_AND_BACK, mode);
	glLineWidth(0.5f);
	obj->draw();

	return true;
}

int Draw::drawing(Room* room, std::vector <Shape*> objects){

		int result = 1;
		
		start = SDL_GetTicks();
		while (SDL_PollEvent(&event)) {
			switch (event.type) {
			case SDL_QUIT:
				return -1;
				break;
			case SDL_KEYDOWN:
				switch (event.key.keysym.sym){
				/*l simuloinnin hätälopetus*/
				case SDLK_l: return -1; break;
				/*f ja g kääntää kameran pyörimistä*/
				case SDLK_f: cameraRotatingLeft = true; cameraRotatingRight = false; break;
				case SDLK_g: cameraRotatingRight = true; cameraRotatingLeft = false; break;
				/* Space lopettaa kameran pyörimisen */
				case SDLK_h: cameraRotatingRight = false; cameraRotatingLeft = false; break;
				/*Luo uusia palloja*/
				case SDLK_LEFT: result = 2; break;
				case SDLK_UP: result = 3; break;
				case SDLK_RIGHT: result = 4; break;
				case SDLK_DOWN: result = 5; break;
				case SDLK_SPACE: result = 6; break;
				}
				break;
			}
		}

		/*Kameran pyörintä*/
		if (cameraRotatingLeft==true){
			cameraRotation += 0.25f;
		}
		else if (cameraRotatingRight == true){
			cameraRotation -= 0.25f;
		}

		/*Nollaus*/
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		/*Piirretään taustan textuuri*/
		if (!drawBackgroundTexture()){
			return -1;
		}

		/*Piirretään huone*/
		if (!drawRoom(room, GL_LINE)){
			return -1;
		}

		/*Piirretään kappalleita*/
		for (int j = 0; j < (int)objects.size(); j++){
			/*Piirretään kappale*/
			drawSphere(objects[j], GL_LINE);
		}

		/*Vaihedaan frame*/
		glEnd();
		SDL_GL_SwapWindow(window);

		/*Rajoitetaan ruudunpäivitys haluttuun suuruuteen*/
		if (1000 / FPS > SDL_GetTicks() - start){
			SDL_Delay(1000 / FPS - (SDL_GetTicks() - start));
		}
		
		return result;
}