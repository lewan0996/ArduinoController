// Procedure.h

#ifndef _PROCEDURE_h
#define _PROCEDURE_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

#include "Commands/Command.h";
#include "CommandFactory.h";
#include <ArduinoJson.h>

class Procedure {
public:
	Procedure(CommandFactory* commandFactory);
	~Procedure();
	void Execute();
	void LoadJson(char* procedureJson);
	std::vector<Command*> Commands;
private:
	CommandFactory * _commandFactory;
};

#endif

