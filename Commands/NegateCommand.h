#pragma once
#include "Command.h"
class NegateCommand :
	public Command
{
public:
	NegateCommand(uint8_t pinNumber);
	~NegateCommand();
	void Execute();
private:
	uint8_t _pinNumber;
};

