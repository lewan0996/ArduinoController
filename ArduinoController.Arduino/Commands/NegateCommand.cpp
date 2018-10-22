#include "NegateCommand.h"

NegateCommand::NegateCommand(command_args* args) : command(args)
{
	_pinNumber = args->pin_number;
}

NegateCommand::~NegateCommand()
= default;

void NegateCommand::execute()
{
	Serial.print("Executing Negate");
	Serial.print(" to pin ");	
	Serial.println(_pinNumber);

	pinMode(_pinNumber, OUTPUT);
	digitalWrite(_pinNumber, !digitalRead(_pinNumber));

	Serial.println("Negate Done.");
}
