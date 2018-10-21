#include "Procedure.h"

Procedure::Procedure(CommandFactory* command_factory)
{
	_commandFactory = command_factory;
}

Procedure::~Procedure()
{
	for (auto& command : Commands)
	{
		delete command;
	}
}

void Procedure::execute()
{	
	for (auto& command : Commands)
	{
		command->Execute();
	}	
}

void Procedure::load_json(const char * procedure_json)
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
		auto* args = new CommandArgs();
		args->Duration = command_json["duration"];
		args->Order = command_json["order"];
		args->PinNumber = command_json["pinNumber"];
		args->Value = command_json["value"];

		auto command = _commandFactory->CreateCommand(command_name, args);

		if (command == nullptr)
		{
			Serial.println("Procedure json is invalid - there is no such command");
			return;
		}
				
		Commands.push_back(command);
	}

	std::sort(Commands.begin(), Commands.end(), Command::Compare);
	isValid = true;
}
