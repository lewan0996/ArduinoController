#include "CommandFactory.h"

command_factory::command_factory()
{
	_commands.insert(
		{		
			{"DigitalWrite",[](command_args* args)->command* {return new digital_write_command(args); }},
			{"AnalogWrite",[](command_args* args)->command* {return new analog_write_command(args); }},
			{"Negate",[](command_args* args)->command* {return new negate_command(args); } },
			{"Wait",[](command_args* args)->command* {return new wait_command(args); } }
		}
	);
}

command* command_factory::create_command(const char* command_name, command_args* args)
{
	const auto command = _commands[command_name];
	if (!command)
	{
		return nullptr;
	}
	return (command)(args);	
}
