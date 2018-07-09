#include "AnalogWriteCommand.h"

AnalogWriteCommand::AnalogWriteCommand(CommandArgs* args) :Command(args)
{
	_pinNumber = args->PinNumber;
	_value = args->Value;
}

AnalogWriteCommand::~AnalogWriteCommand()
{
}

void AnalogWriteCommand::Execute()
{
	Serial.print("Executing analog write ");
	Serial.print(_value);
	Serial.print(" to pin ");
	Serial.println(_pinNumber);

	pinMode(_pinNumber, OUTPUT);
	analogWrite(_pinNumber, _value);

	Serial.println("AnalogWrite Done.");
}

void AnalogWriteCommand::SetPinNumber(uint8_t pinNumber)
{
	_pinNumber = pinNumber;
}

void AnalogWriteCommand::SetValue(uint8_t value)
{
	_value = value;
}
