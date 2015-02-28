#pragma once
#include "stdafx.h"
#include "Shape.h"
#include "Sphere.h"
#include "Room.h"
/*Fysiikka funktiot*/
interface Physics{
	
private:

	/*Törmäystarkistus liikkuvien kappaleiden kesken*/
		/*Etsi lähin pallo johon törmätään*/
		Shape* findNearestCollision(std::vector<Shape*> moving_objects, int num);
		/*Tarkistaa törmääkö kaksi palloa*/
		bool SphereCollisions(Shape* sphereOne, Shape* sphereTwo);
		/*Kahden pallon yhteisen nopeuden ja törmäyksen normaalivektorin N pistetulo*/
		float dotProductWithNormalvector(Shape* sphereOne, Shape* sphereTwo);

	/*Törmäyksen jälkeisen nopeuden asettaminen*/
		/*Kahden pallon törmäyksen jälkeinen nopeus*/
		void setAfterCollisionSpeed(Shape* sphereOne, Shape* sphereTwo);
		/*Kahden kappaleen välinen impulssi*/
		float getImpulse(Shape* sphereOne, Shape* sphereTwo);

	/*Tekee tsekin seinien kanssa yksittäiselle pallolle*/
		void SphereWallCheck(Shape* sphere, Room* room);

	/*gravitaatio*/
		void Gravitation(Shape* sphere, Room* room);

public:

	/*Takistaa liikkuvien kappaleiden törmäykset toisiinsa ja kiinteisiin kappaleisiin.
	Sekä palauttaa osoittimen kaikkiin liikkuviin kappaleisiin jotka törmäsi johonkin*/
	std::vector<Shape*> CheckPhysics(std::vector<Shape*> moving_objects, std::vector<Room*> solid_objects);

};