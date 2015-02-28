#include "Sphere.h"

Sphere::Sphere(float _x, float _y, float _z, float _mass, float _radius){
	position_vector = new Position(_x, _y, _z);
	speed_vector = new Position();
	rotation_vector = new Position(1.0f,0.0f,0.0f);

	set_angular_momentum();

	mass = _mass;

	radius = _radius;
}

Sphere::~Sphere(){
	free(position_vector);
	free(speed_vector);
	free(rotation_vector);
}

void Sphere::draw(){
	glTranslatef(position_vector->X(), position_vector->Y(), position_vector->Z());
	glRotatef(angle, rotation_vector->X(), rotation_vector->Y(), rotation_vector->Z());

	float lats = 12.0f;
	float longs = 12.0f;
	int i, j;
	for (i = 0; i <= lats; i++) {
		float lat0 = (float)M_PI * (-0.5f + (float)(i - 1) / lats);
		float z0 = sin(lat0) * radius;
		float zr0 = cos(lat0) * radius;

		float lat1 = (float)M_PI * (-0.5f + (float)i / lats);
		float z1 = sin(lat1) * radius;
		float zr1 = cos(lat1) * radius;

		glBegin(GL_QUAD_STRIP);
		glColor3f(1.0, 0.0, 1.0);
		for (j = 0; j <= longs; j++) {
			float lng = 2 * (float)M_PI * (float)(j - 1) / longs;
			float x = cos(lng);
			float y = sin(lng);

			glNormal3f(x * zr0, y * zr0, z0);
			glVertex3f(x * zr0, y * zr0, z0);
			glNormal3f(x * zr1, y * zr1, z1);
			glVertex3f(x * zr1, y * zr1, z1);
		}
		glEnd();
	}
}