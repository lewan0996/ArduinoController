#include "DigitalWriteCommand.h"

DigitalWriteCommand::DigitalWriteCommand(command_args* args) : command(args)
{
	_pinNumber = args->pin_number;
	_value = args->value;
}

DigitalWriteCommand::~DigitalWriteCommand()
= default;

void DigitalWriteCommand::set_pin_number(const uint8_t pin_number)
{
	_pinNumber = pin_number;
}

void DigitalWriteCommand::set_value(const uint8_t value)
{
	_value = value;
}

void DigitalWriteCommand::execute()
{
	Serial.print("Executing digital write ");
	Serial.print(_value);
	Serial.print(" to pin ");
	Serial.println(_pinNumber);

	pinMode(_pinNumber, OUTPUT);
	digitalWrite(_pinNumber, _value);

	Serial.println("DigitalWrite Done.");
}
