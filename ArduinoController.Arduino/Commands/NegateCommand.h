#include "Command.h"

#ifndef _NEGATE_COMMAND_h
#define _NEGATE_COMMAND_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

class NegateCommand :
	public command
{
public:	
	NegateCommand(command_args* args);
	~NegateCommand();
	void execute();
private:
	uint8_t _pinNumber;
};

#endif