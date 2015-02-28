#pragma once
#include "stdafx.h"
#include "Shape.h"
/*Sis‰lt‰‰ palllon ominaisuuksia 
sek‰ konstruktorin*/
class Sphere : public Shape{

protected:

	/*Pallon s‰de metrein‰*/
	float radius;

	/*Hitausmomentti on vakio.
	Asetetaan hitausmomentti ilmentym‰n konstruktorissa.*/
	void Shape::set_angular_momentum(){
		angular_momentum = (2 / 5) * mass * pow(radius, 2.0f);
	}

public:

	/*Pallon konstruktori joka asettaa oletusarvoisena massaksi 1 ja s‰teeksi 0.1*/
	Sphere(float _x = 0.0f, float _y = 0.0f, float _z = 0.0f, float _mass=1.0f, float _radius = 0.2f);
	~Sphere();

	/*Pallon piirto*/
	void Shape::draw();

	/*Pallo oliolta voidaan getata ainoastaan s‰de johon sis‰ltyy varoraja*/
	float Shape::get_radius(){
		return radius + safetyzone;
	}

};