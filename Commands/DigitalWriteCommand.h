#include "Command.h"
#ifndef _DIGITAL_WRITE_COMMAND_h
#define _DIGITAL_WRITE_COMMAND_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

class DigitalWriteCommand :
	public Command
{
public:	
	DigitalWriteCommand(CommandArgs* args);
	~DigitalWriteCommand();
	void SetPinNumber(uint8_t pinNumber);
	void SetValue(uint8_t value);
	void Execute();
private:
	uint8_t _pinNumber;
	uint8_t _value;
};

#endif

