#include "AnalogWriteCommand.h"

AnalogWriteCommand::AnalogWriteCommand(uint8_t pinNumber, uint8_t value)
{
	_pinNumber = pinNumber;
	_value = value;
}

AnalogWriteCommand::~AnalogWriteCommand()
{
}

void AnalogWriteCommand::Execute()
{
	pinMode(_pinNumber, OUTPUT);
	analogWrite(_pinNumber, _value);
}

void AnalogWriteCommand::SetPinNumber(uint8_t pinNumber)
{
	_pinNumber = pinNumber;
}

void AnalogWriteCommand::SetValue(uint8_t value)
{
	_value = value;
}
