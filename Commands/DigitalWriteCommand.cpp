#include "DigitalWriteCommand.h"

DigitalWriteCommand::DigitalWriteCommand()
{
}


DigitalWriteCommand::~DigitalWriteCommand()
{
}

void DigitalWriteCommand::SetPinNumber(uint8_t pinNumber)
{
	_pinNumber = pinNumber;
}

void DigitalWriteCommand::SetValue(uint8_t value)
{
	_value = value;
}

void DigitalWriteCommand::Execute()
{
	pinMode(_pinNumber, OUTPUT);
	digitalWrite(_pinNumber, _value);
}
