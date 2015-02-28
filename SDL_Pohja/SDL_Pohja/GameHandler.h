#pragma once
#include "stdafx.h"
#include "Sphere.h"
#include "Shape.h"
#include "Room.h"
#include "Physics.h"
#include "Draw.h"
class GameHandler : public Physics{

public:

	static GameHandler* handler;
	Draw* drawer;
	Room* room;
	std::vector<Shape*> objects;
	std::vector<Room*> solid_objects;
	bool flag=true;
	float roomsize = 7.5f;

	GameHandler(){
		/*Alustetaan piirros*/
		drawer = Draw::init();

		/*Alustetaan huone*/
		room = new Room(roomsize * 2, roomsize, roomsize);
		
		solid_objects.resize(1);
		solid_objects[0] = room;

	};

	~GameHandler();

	/*Lisää kaksi palloa huoneeseen*/
	void add_Spheres(float  i, float j, float k,float size=2.0f){

		/*Luodaan spheret*/
		Sphere* sphereOne = new Sphere(roomsize*i, roomsize*j / 2, roomsize*k / 2, 10.0f *size*2.0f, 0.5f*size/2);
		Sphere* sphereTwo = new Sphere(-roomsize*i, -roomsize*j / 2, -roomsize*k / 2, 10.0f *size*2.0f, 0.5f*size/2);
		sphereTwo->get_direction_of_rotation()->set_new_parameters(0.0f, 1.0f, -1.0f);
		sphereTwo->get_direction_of_rotation()->set_new_parameters(0.0f, 1.0f, 0.0f);
		sphereOne->get_speed_vector()->set_new_parameters(-0.1f*i, -0.1f*j, -0.1f*k);
		sphereTwo->get_speed_vector()->set_new_parameters(0.1f*i, 0.1f*j, 0.1f*k);

		/*Varataan sopiva tila sphereille*/
		std::vector<Shape*> old_spheres = objects;
		objects.resize( (int)objects.size() + 2 );

		/*Lisätään uudet spheret*/
		objects[(int)objects.size() - 2] = sphereOne;
		objects[(int)objects.size() - 1] = sphereTwo;
	}
	void add_Sphere(float  i, float j, float k){

		/*Luodaan spheret*/
		Sphere* sphereOne = new Sphere(roomsize*i, (roomsize*j / 4), roomsize*k / 2,1.0f,0.25);
		sphereOne->get_direction_of_rotation()->set_new_parameters(0.0f, 1.0f, -1.0f);
		sphereOne->get_speed_vector()->set_new_parameters(-0.1f*i, -0.1f*j, -0.1f*k);

		/*Varataan sopiva tila sphereille*/
		std::vector<Shape*> old_spheres = objects;
		objects.resize((int)objects.size() + 1);

		/*Lisätään uudet spheret*/
		objects[(int)objects.size() - 1] = sphereOne;
	}

	static GameHandler* LoadGame(){
		if ( !handler ){
			handler = new GameHandler();
		}
		return handler;
	};

	void Start(){

		int drawing=1;
		bool moving=true;

		/*Varmistetaan ettei pallot ole heti alussa päällekkäin*/
		Physics::CheckPhysics(objects, solid_objects);
		
		while ( ( drawing = drawer->drawing(room, objects) ) >= 0 ){

			/*Painallus vasemmalle*/
			if (drawing == 2){ add_Spheres(1,0,0); }
			/*Painallus ylös*/
			if (drawing == 3){ add_Sphere(0,1,0); }
			/*Painallus oikealle*/
			if (drawing == 4){ add_Spheres(0, 0, 1); }
			/*Painallus alas*/
			if (drawing == 5){ add_Spheres(1, 0, 0,5); }
			/*Painallus välilyönti*/
			if (drawing == 6){ 
				if (moving){ 
					moving = false; 
				}else { 
					moving = true;
				}
			}
			if (moving){
				/*Check colissions*/
				Physics::CheckPhysics(objects, solid_objects);

				/*Liikutetaan kappaletta*/
				for (auto element : objects){
					element->auto_move();
				}
			}

		}
	}

};