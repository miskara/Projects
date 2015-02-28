#include "Room.h"

void Room::drawRoom(){
	glBegin(GL_QUADS);
	// front face
	glColor3f(1.0, 0.0, 0.0);
	glVertex3f(this->width / 2, this->height / 2, this->depth / 2);
	glVertex3f(-this->width / 2, this->height / 2, this->depth / 2);
	glVertex3f(-this->width / 2, -this->height / 2, this->depth / 2);
	glVertex3f(this->width / 2, -this->height / 2, this->depth / 2);
	// left face
	glColor3f(0.0, 1.0, 0.0);
	glVertex3f(-this->width / 2, this->height / 2, this->depth / 2);
	glVertex3f(-this->width / 2, -this->height / 2, this->depth / 2);
	glVertex3f(-this->width / 2, -this->height / 2, -this->depth / 2);
	glVertex3f(-this->width / 2, this->height / 2, -this->depth / 2);
	// back face
	glColor3f(0.0, 0.0, 1.0);
	glVertex3f(this->width / 2, this->height / 2, -this->depth / 2);
	glVertex3f(-this->width / 2, this->height / 2, -this->depth / 2);
	glVertex3f(-this->width / 2, -this->height / 2, -this->depth / 2);
	glVertex3f(this->width / 2, -this->height / 2, -this->depth / 2);
	// right face
	glColor3f(1.0, 1.0, 0.0);
	glVertex3f(this->width / 2, this->height / 2, this->depth / 2);
	glVertex3f(this->width / 2, -this->height / 2, this->depth / 2);
	glVertex3f(this->width / 2, -this->height / 2, -this->depth / 2);
	glVertex3f(this->width / 2, this->height / 2, -this->depth / 2);
	// top face
	glColor3f(1.0, 0.0, 1.0);
	glVertex3f(this->width / 2, this->height / 2, this->depth / 2);
	glVertex3f(-this->width / 2, this->height / 2, this->depth / 2);
	glVertex3f(-this->width / 2, this->height / 2, -this->depth / 2);
	glVertex3f(this->width / 2, this->height / 2, -this->depth / 2);
	// bottom face
	glColor3f(0.0, 1.0, 1.0);
	glVertex3f(this->width / 2, -this->height / 2, this->depth / 2);
	glVertex3f(-this->width / 2, -this->height / 2, this->depth / 2);
	glVertex3f(-this->width / 2, -this->height / 2, -this->depth / 2);
	glVertex3f(this->width / 2, -this->height / 2, -this->depth / 2);
	glEnd();
}