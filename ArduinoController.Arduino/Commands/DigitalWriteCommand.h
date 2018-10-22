#include "Command.h"
#ifndef _DIGITAL_WRITE_COMMAND_h
#define _DIGITAL_WRITE_COMMAND_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

class DigitalWriteCommand :
	public command
{
public:	
	DigitalWriteCommand(command_args* args);
	~DigitalWriteCommand();
	void set_pin_number(uint8_t pinNumber);
	void set_value(uint8_t value);
	void execute();
private:
	uint8_t _pinNumber;
	uint8_t _value;
};

#endif

