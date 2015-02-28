#include "stdafx.h"
#include "GameHandler.h"


int main(int argc, char** argv){
	 
	static GameHandler *handler = GameHandler::LoadGame();
	handler->Start();

	return 0;
}