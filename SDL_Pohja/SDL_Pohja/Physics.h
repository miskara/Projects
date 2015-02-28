#pragma once
#include "stdafx.h"
#include "Shape.h"
#include "Sphere.h"
#include "Room.h"
/*Fysiikka funktiot*/
interface Physics{
	
private:

	/*T�rm�ystarkistus liikkuvien kappaleiden kesken*/
		/*Etsi l�hin pallo johon t�rm�t��n*/
		Shape* findNearestCollision(std::vector<Shape*> moving_objects, int num);
		/*Tarkistaa t�rm��k� kaksi palloa*/
		bool SphereCollisions(Shape* sphereOne, Shape* sphereTwo);
		/*Kahden pallon yhteisen nopeuden ja t�rm�yksen normaalivektorin N pistetulo*/
		float dotProductWithNormalvector(Shape* sphereOne, Shape* sphereTwo);

	/*T�rm�yksen j�lkeisen nopeuden asettaminen*/
		/*Kahden pallon t�rm�yksen j�lkeinen nopeus*/
		void setAfterCollisionSpeed(Shape* sphereOne, Shape* sphereTwo);
		/*Kahden kappaleen v�linen impulssi*/
		float getImpulse(Shape* sphereOne, Shape* sphereTwo);

	/*Tekee tsekin seinien kanssa yksitt�iselle pallolle*/
		void SphereWallCheck(Shape* sphere, Room* room);

	/*gravitaatio*/
		void Gravitation(Shape* sphere, Room* room);

public:

	/*Takistaa liikkuvien kappaleiden t�rm�ykset toisiinsa ja kiinteisiin kappaleisiin.
	Sek� palauttaa osoittimen kaikkiin liikkuviin kappaleisiin jotka t�rm�si johonkin*/
	std::vector<Shape*> CheckPhysics(std::vector<Shape*> moving_objects, std::vector<Room*> solid_objects);

};