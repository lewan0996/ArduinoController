#include "CommandFactory.h"

CommandFactory::CommandFactory()
{
	_commands.insert(
		{		
			{"DigitalWrite",[](CommandArgs* args)->Command* {return new DigitalWriteCommand(args); }},
			{"AnalogWrite",[](CommandArgs* args)->Command* {return new AnalogWriteCommand(args); }},
			{"Negate",[](CommandArgs* args)->Command* {return new NegateCommand(args); } },
			{"Wait",[](CommandArgs* args)->Command* {return new WaitCommand(args); } }
		}
	);
}


CommandFactory::~CommandFactory()
{

}

Command* CommandFactory::CreateCommand(const char* commandName, CommandArgs* args)
{
	auto command = _commands[commandName];
	if (!command)
	{
		return NULL;
	}
	return (command)(args);	
}
