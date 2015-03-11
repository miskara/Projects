#include "Physics.h"

std::vector<Shape*> Physics::CheckPhysics(std::vector<Shape*> moving_objects, std::vector<Room*> solid_objects){

	/*Puuttuu vielä se tuplatsekkaus ettei PALLON NOPEUTTA MUUTETA KAHDESTI*/

	/*Pallojen keskenäinen törmäily ja niiden aiheuttamat nopeuden muutokset.
	Tarkastetaan kaikkien palloparien väliset etäisyydet ja mahdolliset törmäykset.*/
	/*Huomioon otetaan ainoastaan lähin törmäys per pallo. Eli törmäys joka on eniten päässyt pallon sisälle 
	tai eniten pallon törmäysalueen eli safety zonen sisällä. Sekä varmistamme ettei pallon nopeutta muuteta kahdesti.*/
	
	/*TÄMÄ ALGORITMI ON SUOLESTA*/
	Shape* nearestCollisionSphere;
	for (int j = 0; j < (float)moving_objects.size(); j++){

		Gravitation(moving_objects[j], solid_objects[0]);
		SphereWallCheck(moving_objects[j], solid_objects[0]);
		/*HIDASTAAA OHJELMAA!!!! POISTETTAVA!!!!!!!!!!!!!!!!!!!
		!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/

		nearestCollisionSphere = findNearestCollision(moving_objects, j);
		if (nearestCollisionSphere != NULL){
			/*Lopulta saamme asettaa uudet nopeudet palloille*/
			setAfterCollisionSpeed(moving_objects[j], nearestCollisionSphere);
		}
		

	}
	


	//for (int j = 0; j < (float)moving_objects.size(); j++){
	//	nearestCollisionSphere = findNearestCollision(moving_objects, j);
	//	if (nearestCollisionSphere != NULL){
	//		/*Lopulta saamme asettaa uudet nopeudet palloille*/
	//		setAfterCollisionSpeed(moving_objects[j], nearestCollisionSphere);
	//	}

	//}

	return moving_objects;
}

Shape* Physics::findNearestCollision(std::vector<Shape*> moving_objects, int num){
	float shortest_distance = NULL;
	float new_distance;
	int nearest = -1;
	/*Etsitään lähin tapahtunut törmäys*/
	for (int i = 0; i < (int)moving_objects.size(); i++){
		if (num != i){
			new_distance = moving_objects[num]->get_position_vector()->distance_to(moving_objects[i]->get_position_vector());
			if (new_distance < shortest_distance || shortest_distance == NULL){
				/*Otetaan huomioon vain todelliset törmäykset*/
				if (SphereCollisions(moving_objects[num], moving_objects[i])){
					shortest_distance = new_distance;
					nearest = i;
				}
			}
		}
	}

	if (nearest == -1){
		return NULL;
	}

	return moving_objects[nearest];
}

bool Physics::SphereCollisions(Shape* sphereOne, Shape* sphereTwo){
	/*Lasketaan säteiden summa*/
	float combinedRadiuses = sphereOne->get_radius() + sphereTwo->get_radius();
	float distance = sphereOne->get_position_vector()->distance_to(sphereTwo->get_position_vector());

	/*Verrataan kappaleiden etäisyyttä ja säteiden summaa*/
	if (distance <= combinedRadiuses){
		/*Siirretään palloja takaisinpäin, etteivät päädy sisäkkäin*/
		float distance_to_back = (combinedRadiuses - distance) / 2;
		/*Lasketaan kuinka paljon kutakin komponenttia siirretään taakse*/
		Position* collisionDirection = sphereOne->get_position_vector()->direction_vector_to_position(sphereTwo->get_position_vector());
		(*collisionDirection) *= distance_to_back;
		/*Tehdään siirto*/
		(*sphereOne->get_position_vector()) += collisionDirection;
		(*sphereTwo->get_position_vector()) -= collisionDirection;

		/*Tsekataan kulkeeko pallot kohti toisiaan vai ei
		jos kulkevat voidaan pallojen suuntaa muuttaa.
		Näin vältytään pallojen sisäkkäin takertumiselta, jos pallot
		ovat kuitenkin päätyneet sisäkkäin*/
		if (dotProductWithNormalvector(sphereOne, sphereTwo) < 0){
			return true;
		}else{
			std::cout << "WARNING: Pallot kulkee sisäkkäin! \n";
		}
	}
	return false;
}

float Physics::dotProductWithNormalvector(Shape* sphereOne, Shape* sphereTwo){
	/*Kahden pallon välisen nopeuden pistetulo normaalivektorin N kanssa*/

	Position* pos = vector_subtraction(sphereOne->get_speed_vector(), sphereTwo->get_speed_vector());
	float result = pos->dot_product(sphereOne->get_position_vector()->direction_vector_to_position(sphereTwo->get_position_vector()));

	if (result == 0){
		std::cout << "Physics::dot_product Pallojen keskipisteet päällekkäin. (Pistetulo nolla) \n";
	}
	return result;
}

void Physics::setAfterCollisionSpeed(Shape* sphereOne, Shape* sphereTwo){
	/*Lasketaan impulssi*/
	float impulse = getImpulse(sphereOne, sphereTwo);

	Position* collisionDirection = sphereOne->get_position_vector()->direction_vector_to_position(sphereTwo->get_position_vector());
	(*collisionDirection) *= (impulse / sphereOne->get_mass());

	/*Nopeudet pallolle yksi*/
	(*sphereOne->get_speed_vector()) += collisionDirection;

	/*Nopeudet pallolle kaksi*/
	(*sphereTwo->get_speed_vector()) -= collisionDirection;

	/*Tulostaa kahden pallon systeemin liike-energian. 
	Energiaa ei saa hävitä tai tulla lisää!!!!*/
	float energy_1 = sphereOne->get_mass()*pow(sphereOne->get_speed_vector()->length(), 2.0f);
	float energy_2 = sphereTwo->get_mass()*pow(sphereTwo->get_speed_vector()->length(), 2.0f);
	float result = (energy_1+energy_2)/2;
	std::cout << "Kahden pallon liike-energia " << result << "\n";

	/*Asetetaan uudet kulmanopeudet*/
	//sphereOne->setAngularVelocity();
	//sphereTwo->setAngularVelocity();
}

float Physics::getImpulse(Shape* sphereOne, Shape* sphereTwo){
	/*Kahden kappaleen välinen impulssi*/
	/*Sysäyskerroin saa arvokseen kappaleiden elastisuuskertoimen keskiarvon*/
	float kerroin = -1.0f + ((sphereOne->get_elasticity() + sphereTwo->get_elasticity()) / 2.0f);

	float osoittaja = dotProductWithNormalvector(sphereOne, sphereTwo);
	float nimittäjä = (1.0f / sphereOne->get_mass()) + (1.0f / sphereTwo->get_mass());

	if (nimittäjä == 0 || kerroin == 0){ std::cout << "ERROR: Physics::getImpulse \n"; return -1; }
	if (osoittaja == 0){ return 0; }
	if (osoittaja > 0){ std::cout << "Physics::getImpulse Odottamaton impulssin arvo (POSITIIVINEN)\n"; }

	return  kerroin * (osoittaja / nimittäjä);
}

void Physics::SphereWallCheck(Shape* sphere, Room* room){
	/*Seinätörmäys*/
	/*Käännetään pallojen suuntaa jo pikkuriikkisen verran ennen seinää.
	SafetyzoneWALL määrittelee pallolle kuinka paljon ennen.*/
	float radius = sphere->get_radius();
	float elasticy = sphere->get_elasticity();
	
	if (sphere->get_position_vector()->X() >= (room->Width() / 2 - radius)){ //check x +
		if (sphere->get_speed_vector()->X() > 0){
			sphere->get_speed_vector()->set_new_parameters(sphere->get_speed_vector()->X() * elasticy, sphere->get_speed_vector()->Y(), sphere->get_speed_vector()->Z());
		}
	}
	else if (sphere->get_position_vector()->X() <= (-room->Width() / 2 + radius)){ //check x -
		if (sphere->get_speed_vector()->X() < 0){
			sphere->get_speed_vector()->set_new_parameters(sphere->get_speed_vector()->X()  * elasticy, sphere->get_speed_vector()->Y(), sphere->get_speed_vector()->Z());
		}
	}
	else if (sphere->get_position_vector()->Y() >= (room->Height() / 2 - radius)){ //check y +
		if (sphere->get_speed_vector()->Y() > 0){
			sphere->get_speed_vector()->set_new_parameters(sphere->get_speed_vector()->X(), sphere->get_speed_vector()->Y() * elasticy, sphere->get_speed_vector()->Z());
		}
	}
	else if (sphere->get_position_vector()->Y() <= (-room->Height() / 2 + radius)){ //check y -
		if (sphere->get_speed_vector()->Y() < 0){
			if (0.01f < sqrt(pow(sphere->get_speed_vector()->Y(),2.0f)) ){
				sphere->get_speed_vector()->set_new_parameters(sphere->get_speed_vector()->X(), sphere->get_speed_vector()->Y() * elasticy, sphere->get_speed_vector()->Z());
			}else{
				sphere->get_speed_vector()->set_new_parameters(sphere->get_speed_vector()->X(), 0.0f, sphere->get_speed_vector()->Z());
			}
		}
	}
	else if (sphere->get_position_vector()->Z() >= (room->Depth() / 2 - radius)){ //check z +
		if (sphere->get_speed_vector()->Z() > 0){
			sphere->get_speed_vector()->set_new_parameters(sphere->get_speed_vector()->X(), sphere->get_speed_vector()->Y(), sphere->get_speed_vector()->Z() * elasticy);
		}
	}
	else if (sphere->get_position_vector()->Z() <= (-room->Depth() / 2 + radius)){ //check z -
		if (sphere->get_speed_vector()->Z() < 0){
			sphere->get_speed_vector()->set_new_parameters(sphere->get_speed_vector()->X(), sphere->get_speed_vector()->Y(), sphere->get_speed_vector()->Z() * elasticy);
		}
	}
}

void Physics::Gravitation(Shape* sphere, Room* room){
	float g = 0.00981f;

	if ( sphere->get_position_vector()->Y() < 0){
		if ( (sphere->get_position_vector()->Y() - sphere->get_radius()) < (-room->Height() / 2)){
			// do nothin
		}else{
			sphere->get_speed_vector()->set_new_parameters(sphere->get_speed_vector()->X(), sphere->get_speed_vector()->Y() - g, sphere->get_speed_vector()->Z());
		}
	}else{ 
		sphere->get_speed_vector()->set_new_parameters(sphere->get_speed_vector()->X(), sphere->get_speed_vector()->Y() - g, sphere->get_speed_vector()->Z());
	}
		


}
/*
void Physics::gravitationalPull(){
	float F;
	float fx=0.0f;
	float fy=0.0f;
	float fz=0.0f;
	float ax = 0.0f;
	float ay = 0.0f;
	float az = 0.0f;


	for (int i = 0; i < (float)spheres.size() ; i++){
		for (int j = 0; j < (float)spheres.size(); j++){
			if (i!=j){
				F = (-BigG*spheres[i]->get_mass()*spheres[j]->get_mass()) / pow(distance(spheres[i], spheres[j]),2.0f);
				fx += getNX(spheres[i], spheres[j])*F;
				fy += getNY(spheres[i], spheres[j])*F;
				fz += getNZ(spheres[i], spheres[j])*F;
			}
		}
		// kiihdytetaan
		ax = fx / spheres[i]->get_mass();
		ay = fy / spheres[i]->get_mass();
		az = fz / spheres[i]->get_mass();

		std::cout << ax << "x " << ay << "y " << az << " pallojen aiheuttama kiihtyvyys \n";

		spheres[i]->setSpeed(spheres[i]->get_speed_vectorX() + (ax), spheres[i]->get_speed_vectorY() + (ay), spheres[i]->get_speed_vectorZ() + (az));

		fx = 0.0f;
		fy = 0.0f;
		fz = 0.0f;
	}
}*/
