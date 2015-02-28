#pragma once
#include "stdafx.h"

class Room {

private:

	/*Annetaan huoneelle ainoastaan positiivisia mittoja*/
	float width = 0.0f, height = 0.0f, depth = 0.0f;

public:

	Room(float _width, float _height, float _depth){
		width = sqrt(pow(_width, 2.0f));
		height = sqrt(pow(_height, 2.0f));
		depth = sqrt(pow(_depth, 2.0f));
	};

	void drawRoom();

	/*Getterit*/
	float Width(){ return width; }
	float Height(){ return height; }
	float Depth(){ return depth; }
};