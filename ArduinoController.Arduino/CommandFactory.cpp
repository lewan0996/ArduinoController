#include "CommandFactory.h"

CommandFactory::CommandFactory()
{
	_commands.insert(
		{		
			{"DigitalWrite",[](command_args* args)->command* {return new DigitalWriteCommand(args); }},
			{"AnalogWrite",[](command_args* args)->command* {return new AnalogWriteCommand(args); }},
			{"Negate",[](command_args* args)->command* {return new NegateCommand(args); } },
			{"Wait",[](command_args* args)->command* {return new WaitCommand(args); } }
		}
	);
}


CommandFactory::~CommandFactory()
= default;

command* CommandFactory::create_command(const char* command_name, command_args* args)
{
	const auto command = _commands[command_name];
	if (!command)
	{
		return nullptr;
	}
	return (command)(args);	
}
