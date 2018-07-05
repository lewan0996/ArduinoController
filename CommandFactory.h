#include "Commands/Command.h";
#include "Commands/AnalogWriteCommand.h";
#include "Commands/DigitalWriteCommand.h";
#include "Commands/NegateCommand.h";
#include "Commands/WaitCommand.h";
#include "StringHelpers.h";

class CommandFactory
{
public:
	CommandFactory();
	~CommandFactory();
	static Command* CreateCommand(char* commandString);
};


