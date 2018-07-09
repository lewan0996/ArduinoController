#include "WaitCommand.h"

WaitCommand::WaitCommand(CommandArgs* args) : Command(args)
{
	_duration = args->Duration;
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
	Serial.print("Executing Wait for ");	
	Serial.println(_duration);

	delay(_duration);

	Serial.println("Wait done.");
}
