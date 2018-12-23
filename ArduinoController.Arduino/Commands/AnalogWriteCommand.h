#ifndef _ANALOG_WRITE_COMMAND_h
#define _ANALOG_WRITE_COMMAND_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

#include "Command.h"

class analog_write_command :
	public command
{
public:
	explicit analog_write_command(command_args* args);	
	void execute() override;
	void set_pin_number(uint8_t pin_number);
	void set_value(uint8_t value);
private:
	uint8_t pin_number_;
	uint8_t value_;
};

#endif




