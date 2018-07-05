#pragma once
#include "Command.h"
class WaitCommand :
	public Command
{
public:
	WaitCommand(unsigned long duration);
	~WaitCommand();
	void SetDuration(unsigned long duration);
	void Execute();
private:
	unsigned long _duration;
};

