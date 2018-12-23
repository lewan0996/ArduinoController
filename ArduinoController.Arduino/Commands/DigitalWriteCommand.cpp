#include "DigitalWriteCommand.h"

digital_write_command::digital_write_command(command_args* args) : command(args)
{
	pin_number_ = args->pin_number;
	value_ = args->value;
}

digital_write_command::~digital_write_command()
= default;

void digital_write_command::set_pin_number(const uint8_t pin_number)
{
	pin_number_ = pin_number;
}

void digital_write_command::set_value(const uint8_t value)
{
	value_ = value;
}

void digital_write_command::execute()
{
	Serial.print("Executing digital write ");
	Serial.print(value_);
	Serial.print(" to pin ");
	Serial.println(pin_number_);

	pinMode(pin_number_, OUTPUT);
	digitalWrite(pin_number_, value_);

	Serial.println("DigitalWrite Done.");
}
