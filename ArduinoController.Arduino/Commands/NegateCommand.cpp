#include "NegateCommand.h"

NegateCommand::NegateCommand(CommandArgs* args) : Command(args)
{
	_pinNumber = args->PinNumber;
}

NegateCommand::~NegateCommand()
{
}

void NegateCommand::Execute()
{
	Serial.print("Executing Negate");
	Serial.print(" to pin ");	
	Serial.println(_pinNumber);

	pinMode(_pinNumber, OUTPUT);
	digitalWrite(_pinNumber, !digitalRead(_pinNumber));

	Serial.println("Negate Done.");
}
