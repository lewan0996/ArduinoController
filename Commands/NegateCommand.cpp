#include "NegateCommand.h"

NegateCommand::NegateCommand(uint8_t pinNumber)
{
	_pinNumber = pinNumber;
}

NegateCommand::~NegateCommand()
{
}

void NegateCommand::Execute()
{
	pinMode(_pinNumber, OUTPUT);
	digitalWrite(_pinNumber, !digitalRead(_pinNumber));
}
