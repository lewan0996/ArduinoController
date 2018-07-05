#include "WaitCommand.h"

WaitCommand::WaitCommand(unsigned long duration)
{
	_duration = duration;
}

WaitCommand::~WaitCommand()
{
}

void WaitCommand::SetDuration(unsigned long duration)
{
	_duration = duration;
}

void WaitCommand::Execute()
{
	delay(_duration);
}
