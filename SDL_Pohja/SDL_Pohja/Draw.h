#pragma once
#include "stdafx.h"
#include "Sphere.h"
#include "Shape.h"
#include "Room.h"
#include "Physics.h"

class Draw {

private:

	static Draw* drawer;

	Draw();
	~Draw();

	/*Piirr‰ taustakuva*/
	bool drawBackgroundTexture();
	/*Lataa tekstuurit*/
	bool loadTextures();
	/*Lataa tiedoston*/
	SDL_Surface* LoadBMP(char *filename);

	/*Piirr‰ liikkuva kappale*/
	bool drawSphere(Shape* object, GLenum mode);
	/*Piirr‰ kiinte‰ kappale*/
	bool drawRoom(Room* rom, GLenum mode);

public:

	/*Alustus*/
	static Draw* init();

	/*Piirt‰‰ framen*/
	int drawing(Room* room, std::vector <Shape*> objects);

	float getwidthOfScreen();
	float getheightOfScreen();
	float getaspectratio();

};