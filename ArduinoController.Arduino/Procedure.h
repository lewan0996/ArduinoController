// Procedure.h

#ifndef _PROCEDURE_h
#define PROCEDURE_H

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

#include "Commands/Command.h";
#include "CommandFactory.h";
#include <ArduinoJson.h>

class procedure {
public:
	explicit procedure(CommandFactory* command_factory);
	~procedure();
	void execute();
	void load_json(const char* procedure_json);
	std::vector<command*> commands;
	bool is_valid = false;
private:
	CommandFactory * command_factory_;
};

#endif

