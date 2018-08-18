#include "../CommandArgs.h";
#ifndef _COMMAND_h
#define _COMMAND_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif
class Command {
public:	
	Command(CommandArgs* args);
	virtual void Execute() = 0;
	unsigned short Order;
	static bool Compare(Command* x, Command* y);
};
#endif

