#ifndef _COMMAND_ARGS_h
#define _COMMAND_ARGS_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif
class CommandArgs
{
public:
	CommandArgs();
	~CommandArgs();
	uint8_t PinNumber;
	uint8_t Value;
	unsigned long Duration;
	unsigned short Order;
};

#endif