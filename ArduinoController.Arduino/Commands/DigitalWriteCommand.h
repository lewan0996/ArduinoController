#include "Command.h"
#ifndef _DIGITAL_WRITE_COMMAND_h
#define _DIGITAL_WRITE_COMMAND_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

class digital_write_command :
	public command
{
public:
	explicit digital_write_command(command_args* args);
	~digital_write_command();
	void set_pin_number(uint8_t pin_number);
	void set_value(uint8_t value);
	void execute() override;
private:
	uint8_t pin_number_;
	uint8_t value_;
};

#endif

