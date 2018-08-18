#include "Command.h"

#ifndef _WAIT_COMMAND_h
#define _WAIT_COMMAND_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

class WaitCommand :
	public Command
{
public:	
	WaitCommand(CommandArgs* args);
	~WaitCommand();
	void SetDuration(unsigned long duration);
	void Execute();
private:
	unsigned long _duration;
};

#endif