#include "Procedure.h"

Procedure::Procedure(CommandFactory* commandFactory)
{
	_commandFactory = commandFactory;
}

Procedure::~Procedure()
{
	for (int i = 0; i < Commands.size(); i++) 
	{
		delete Commands[i];
	}
}

void Procedure::Execute()
{	
	for (int i=0; i< Commands.size();i++)
	{
		Commands[i]->Execute();
	}	
}

void Procedure::LoadJson(const char * procedureJson)
{
	DynamicJsonBuffer jsonBuffer;

	JsonObject& payload = jsonBuffer.parseObject(procedureJson);

	JsonArray& commandsArray = payload["commands"];

	size_t commandsArraySize = commandsArray.size();

	if (commandsArraySize == 0) 
	{
		Serial.println("Procedure json is invalid - array parse error");		
		return;
	}	

	for (int i = 0; i < commandsArraySize; i++) 
	{
		JsonObject& commandJson = commandsArray[i];
		const char* commandName = commandJson["name"];
		if (commandName == nullptr)
		{
			Serial.println("Procedure json is invalid - command name parse error");
			return;
		}
		CommandArgs* args = new CommandArgs();
		args->Duration = commandJson["duration"];
		args->Order = commandJson["order"];
		args->PinNumber = commandJson["pinNumber"];
		args->Value = commandJson["value"];	

		Command* command = _commandFactory->CreateCommand(commandName, args);

		if (command == NULL)
		{
			Serial.println("Procedure json is invalid - there is no such command");
			return;
		}
				
		Commands.push_back(command);
	}

	std::sort(Commands.begin(), Commands.end(), Command::Compare);
	isValid = true;
}
