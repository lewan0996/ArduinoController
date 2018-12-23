#pragma once
#include "../CommandArgs.h";
#ifndef _COMMAND_h
#define _COMMAND_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif
class command {
public:	
	explicit command(command_args* args);
	virtual void execute() = 0;
	unsigned short order;
	static bool compare(command* x, command* y);
};
#endif

