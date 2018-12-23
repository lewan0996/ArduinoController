#include "AnalogWriteCommand.h"

analog_write_command::analog_write_command(command_args* args) :command(args)
{
	pin_number_ = args->pin_number;
	value_ = args->value;
}

void analog_write_command::execute()
{
	Serial.print("Executing analog write ");
	Serial.print(value_);
	Serial.print(" to pin ");
	Serial.println(pin_number_);

	pinMode(pin_number_, OUTPUT);
	analogWrite(pin_number_, value_);

	Serial.println("AnalogWrite Done.");
}

void analog_write_command::set_pin_number(uint8_t pinNumber)
{
	pin_number_ = pinNumber;
}

void analog_write_command::set_value(uint8_t value)
{
	value_ = value;
}
