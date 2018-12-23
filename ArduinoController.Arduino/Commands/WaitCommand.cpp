#include "WaitCommand.h"

wait_command::wait_command(command_args* args) : command(args)
{
	duration_ = args->duration;
}

void wait_command::set_duration(const unsigned long duration)
{
	duration_ = duration;
}

void wait_command::execute()
{
	Serial.print("Executing Wait for ");	
	Serial.println(duration_);

	delay(duration_);

	Serial.println("Wait done.");
}
