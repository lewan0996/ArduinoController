#include "DigitalWriteCommand.h"

DigitalWriteCommand::DigitalWriteCommand(uint8_t pinNumber, uint8_t value)
{
	_pinNumber = pinNumber;
	_value = value;
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
	Serial.print("Executing digital write ");
	Serial.print(_value);
	Serial.print(" to pin ");
	Serial.println(_pinNumber);

	pinMode(_pinNumber, OUTPUT);
	digitalWrite(_pinNumber, _value);

	Serial.println("DigitalWrite Done.");
}
