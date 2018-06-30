#pragma once
#include "Command.h"
class DigitalWriteCommand :
	public Command
{
public:
	DigitalWriteCommand();
	~DigitalWriteCommand();
	void SetPinNumber(uint8_t pinNumber);
	void SetValue(uint8_t value);
	void Execute();
private:
	uint8_t _pinNumber;
	uint8_t _value;
};

