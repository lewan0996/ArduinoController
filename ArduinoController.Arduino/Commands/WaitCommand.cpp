#include "WaitCommand.h"

WaitCommand::WaitCommand(command_args* args) : command(args)
{
	_duration = args->duration;
}

WaitCommand::~WaitCommand()
{
}

void WaitCommand::set_duration(const unsigned long duration)
{
	_duration = duration;
}

void WaitCommand::execute()
{
	Serial.print("Executing Wait for ");	
	Serial.println(_duration);

	delay(_duration);

	Serial.println("Wait done.");
}
