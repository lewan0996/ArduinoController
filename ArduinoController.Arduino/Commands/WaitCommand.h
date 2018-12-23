#include "Command.h"

#ifndef _WAIT_COMMAND_h
#define _WAIT_COMMAND_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

class wait_command :
	public command
{
public:
	explicit wait_command(command_args* args);	
	void set_duration(unsigned long duration);
	void execute() override;
private:
	unsigned long duration_;
};

#endif