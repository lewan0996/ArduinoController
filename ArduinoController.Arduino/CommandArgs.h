#ifndef _COMMAND_ARGS_h
#define _COMMAND_ARGS_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif
class command_args
{
public:	
	uint8_t pin_number;
	uint8_t value;
	unsigned long duration;
	unsigned short order;
};

#endif