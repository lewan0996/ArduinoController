#ifndef _ANALOG_WRITE_COMMAND_h
#define _ANALOG_WRITE_COMMAND_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

#include "Command.h"

class AnalogWriteCommand :
	public Command
{
public:	
	AnalogWriteCommand(CommandArgs* args);
	~AnalogWriteCommand();
	void Execute();
	void SetPinNumber(uint8_t pinNumber);
	void SetValue(uint8_t value);
private:
	uint8_t _pinNumber;
	uint8_t _value;
};

#endif




