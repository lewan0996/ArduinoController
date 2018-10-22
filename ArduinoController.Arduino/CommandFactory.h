#ifndef _COMMAND_FACTORY_h
#define _COMMAND_FACTORY_h

#include "Commands/Command.h";
#include "Commands/AnalogWriteCommand.h";
#include "Commands/DigitalWriteCommand.h";
#include "Commands/NegateCommand.h";
#include "Commands/WaitCommand.h";
#include "StringHelpers.h";
#include "map";
#include "CommandArgs.h";

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

#include <ArduinoJson.h>

class CommandFactory
{
public:
	CommandFactory();
	~CommandFactory();
	command* create_command(const char* commandName, command_args* args);
private:
	std::map<const char*, std::function<command*(command_args* args)>, CompareCStrings> _commands;
};

#endif