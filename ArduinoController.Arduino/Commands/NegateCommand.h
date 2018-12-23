#include "Command.h"

#ifndef _NEGATE_COMMAND_h
#define _NEGATE_COMMAND_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

class negate_command :
	public command
{
public:
	explicit negate_command(command_args* args);	
	void execute() override;
private:
	uint8_t _pinNumber;
};

#endif