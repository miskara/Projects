#pragma once
#include "stdafx.h"
/*Paikkavektori ja vektorioperaatiot*/
class Position{
private:
	/*Koordinaatit piilotettuna*/
	float x, y, z;

public:

	Position(){ x = y = z = 0; };
	Position(float _x, float _y, float _z){ x = _x; y = _y; z = _z; };
	~Position(){ x = y = z = 0; };

	/*Asettaa vektorille uudet arvot*/
	void set_new_parameters(float _x, float _y, float _z){
		x = _x;
		y = _y;
		z = _z;
	}

	/*Koordinaatteihin getterit*/
	float X(){ return x; }
	float Y(){ return y; }
	float Z(){ return z; }

	/*Matemaattiset operaatioit vectoreille*/

		/*Vektorin pituus. Sama kuin etäisyys keskipisteeseen*/
		float length(){
			return sqrt(pow(x, 2.0f) + pow(y, 2.0f) + pow(z, 2.0f));
		}

		/*Etäisyys toiseen pisteeseen*/
		float distance_to(Position* _position2){
			float powx = pow(this->x - _position2->x, 2.0f);
			float powy = pow(this->y - _position2->y, 2.0f);
			float powz = pow(this->z - _position2->z, 2.0f);
			float distance = sqrt(powx + powy + powz);

			if (distance >= 0){
				return distance;
			}

			std::cout << "Position::distance_to ERROR:Distance is negative! \n";
			return -1;
		}

		/*Kahden pisteen välinen suuntavektori*/
		Position* direction_vector_to_position(Position* _positionB){
			float distance = this->distance_to(_positionB);

			if (distance==0){
				std::cout << "Position::directioVectorTo ERROR:dividing by zero!\n";
				return new Position(0.0f, 0.0f, 0.0f);
			}

			float i = (this->x - _positionB->x) / distance;
			float j = (this->y - _positionB->y) / distance;
			float k = (this->z - _positionB->z) / distance;

			return new Position(i,j,k);
		}

		/*Kahden vektorin pistetulo*/
		float dot_product(Position* _position2){
			float i = this->x * _position2->x;
			float j = this->y * _position2->y;
			float k = this->z * _position2->z;
			return i + j + k;
		}

		/*Kahden vektorin ristitulo*/
		Position* cross_product(Position* _position2){
			float i = (this->y * _position2->z) - (this->z * _position2->y);
			float j = (this->z * _position2->x) - (this->x * _position2->z);
			float k = (this->x * _position2->y) - (this->y * _position2->x);
			return new Position(i, j, k);
		}

		/*Vektorien yhteenlasku*/
		void operator+=(Position* _position){
			this->x += _position->x;
			this->y += _position->y;
			this->z += _position->z;
		}

		/*Vektorista vähennys*/
		void operator-=(Position* _position){
			this->x -= _position->x;
			this->y -= _position->y;
			this->z -= _position->z;
		}

		/*Skalaaritulo*/
		void operator*=(float number){
			this->x *= number;
			this->y *= number;
			this->z *= number;
		}

		/*Vektorien vähennyslasku*/
		friend Position* vector_subtraction(Position* positionA, Position* positionB){
			float i = positionA->x - positionB->x;
			float j = positionA->y - positionB->y;
			float k = positionA->z - positionB->z;
			return new Position(i,j,k);
		}
};