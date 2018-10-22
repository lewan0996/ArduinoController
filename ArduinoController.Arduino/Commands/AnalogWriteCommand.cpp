#include "AnalogWriteCommand.h"

AnalogWriteCommand::AnalogWriteCommand(command_args* args) :command(args)
{
	_pinNumber = args->pin_number;
	_value = args->value;
}

AnalogWriteCommand::~AnalogWriteCommand()
= default;

void AnalogWriteCommand::execute()
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
