#pragma once
#include "Command.h"
class AnalogWriteCommand :
	public Command
{
public:
	AnalogWriteCommand(uint8_t pinNumber, uint8_t value);
	~AnalogWriteCommand();
	void Execute();
	void SetPinNumber(uint8_t pinNumber);
	void SetValue(uint8_t value);
private:
	uint8_t _pinNumber;
	uint8_t _value;
};

