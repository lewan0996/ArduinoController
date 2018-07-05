#include "CommandFactory.h"



CommandFactory::CommandFactory()
{
}


CommandFactory::~CommandFactory()
{
}

Command* CommandFactory::CreateCommand(char* commandString)
{
	std::vector<char*> args = StringHelpers::Split(commandString, "_");

	if (strcmp(args[0], "DW") == 0)
	{
		return new DigitalWriteCommand(strtol(args[1], 0, 10), strtol(args[2], 0, 10));
	}

	if (strcmp(args[0], "AW") == 0)
	{
		return new AnalogWriteCommand(strtol(args[1], 0, 10), strtol(args[2], 0, 10));
	}

	if (strcmp(args[0], "N") == 0)
	{
		return new NegateCommand(strtol(args[1], 0, 10));
	}

	if (strcmp(args[0], "W") == 0)
	{
		return new WaitCommand(strtol(args[1], 0, 10));
	}

	return new WaitCommand(0);
}
