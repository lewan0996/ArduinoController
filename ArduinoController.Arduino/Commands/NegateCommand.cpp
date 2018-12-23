#include "NegateCommand.h"

negate_command::negate_command(command_args* args) : command(args)
{
	_pinNumber = args->pin_number;
}

void negate_command::execute()
{
	Serial.print("Executing Negate");
	Serial.print(" to pin ");	
	Serial.println(_pinNumber);

	pinMode(_pinNumber, OUTPUT);
	digitalWrite(_pinNumber, !digitalRead(_pinNumber));

	Serial.println("Negate Done.");
}
