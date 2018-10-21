#include "Procedure.h"

procedure::procedure(CommandFactory* command_factory)
{
	command_factory_ = command_factory;
}

procedure::~procedure()
{
	for (auto& command : commands)
	{
		delete command;
	}
}

void procedure::execute()
{	
	for (auto& command : commands)
	{
		command->execute();
	}	
}

void procedure::load_json(const char * procedure_json)
{
	DynamicJsonBuffer json_buffer;

	auto& payload = json_buffer.parseObject(procedure_json);

	JsonArray& commands_array = payload["commands"];

	size_t commands_array_size = commands_array.size();

	if (commands_array_size == 0) 
	{
		Serial.println("Procedure json is invalid - array parse error");		
		return;
	}	

	for (auto i = 0; i < commands_array_size; i++) 
	{
		JsonObject& command_json = commands_array[i];
		const char* command_name = command_json["type"];
		if (command_name == nullptr)
		{
			Serial.println("Procedure json is invalid - command name parse error");
			return;
		}
		auto* args = new command_args();
		args->duration = command_json["duration"];
		args->order = command_json["order"];
		args->pin_number = command_json["pinNumber"];
		args->value = command_json["value"];

		auto command = command_factory_->create_command(command_name, args);

		if (command == nullptr)
		{
			Serial.println("Procedure json is invalid - there is no such command");
			return;
		}
				
		commands.push_back(command);
	}

	std::sort(commands.begin(), commands.end(), command::compare);
	is_valid = true;
}
