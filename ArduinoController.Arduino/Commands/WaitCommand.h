#include "Command.h"

#ifndef _WAIT_COMMAND_h
#define _WAIT_COMMAND_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

class WaitCommand :
	public command
{
public:	
	WaitCommand(command_args* args);
	~WaitCommand();
	void set_duration(unsigned long duration);
	void execute();
private:
	unsigned long _duration;
};

#endif