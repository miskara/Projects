#pragma once
#include "stdafx.h"
/*Sisältää ainoastaan kappaleesta riippumattomia määreitä.
Eli massakeskipisteen  ominaisuuksia*/
interface Shape {

protected:

	/*Massakeskipisteen koordinaatit huoneen keskipisteeseen nähden*/
	Position* position_vector;

	/*Keskipisteen nopeus*/
	Position* speed_vector;

	/*Pyörimisen suuntavektori*/
	Position* rotation_vector;

	/*Kulmanopeus radiaaneina*/
	float angularVelocity = 5.0f;

	/*Pöyrimiskulma radiaaneina*/
	float angle = 0.0f;

	/*Hitausmomentti*/
	float angular_momentum;

	/*Massa kiloina*/
	float mass;

	/*Kappaleiden elastisuus saa oletuarvokseen -1
	eli kappaleet ovat täysin kimmoisia vakiona*/
	float elasticity = -1.0f;

	/*Kappaleen kitkakerroin saa arvoja välillä 0...+1*/
	float friction = 0.65f;

	/*Hitausmomentti on vakio, mutta kaikille eri muodoille eri.
	Asetetaan hitausmomentti ilmentymän konstruktorissa.*/
	virtual void set_angular_momentum() = 0;

	/*Varoraja kappaleille*/
	float safetyzone = 0.00025f;

public:

	/*Kappaleen piirtofunktio*/
	virtual void draw() = 0;

	/*Kappaleen liikuttaminen ja pyörittäminen nopeuksien mukaan*/
	void auto_move(){
		(*position_vector) += speed_vector;
		angle += angularVelocity;
	}

	/*Asetetaan uusi kulmanopeus*/
	void set_angular_velocity(float _angularVelocity){
		angularVelocity = _angularVelocity;
	}

	/*Elastisuuskertoimen oltava välillä 0 -> -1*/
	void set_elasticity(float _elasticity){
		if (_elasticity <= 0 && _elasticity >= -1){
			elasticity = _elasticity;
		}
	}

	/*Kitkan oltava välillä 0 -> +1*/
	void set_friction(float _friction){
		if (_friction >= 0 && _friction<=1){
			friction = _friction;
		}
	}

	/*Getterit*/
	float get_mass(){
		return mass;
	}

	float get_angular_momentum(){
		return angular_momentum;
	}

	Position* get_position_vector(){
		return position_vector;
	}

	Position* get_speed_vector(){
		return speed_vector;
	}

	Position* get_direction_of_rotation(){
		return rotation_vector;
	}

	float get_angular_velocity(){
		return angularVelocity;
	}

	float get_friction(){
		return friction;
	}

	float get_elasticity(){
		return elasticity;
	}

	virtual float get_radius() = 0;

};