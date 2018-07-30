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

void Procedure::LoadJson(char * procedureJson)
{
	StaticJsonBuffer<200> jsonBuffer;

	JsonArray& commandsArray = jsonBuffer.parseArray(procedureJson);

	size_t commandsArraySize = commandsArray.size();

	if (commandsArraySize == 0) 
	{
		Serial.println("Procedure json is invalid");
		isValid = false;
		return;
	}
	
	isValid = true;

	for (int i = 0; i < commandsArraySize; i++) 
	{
		JsonObject& commandJson = commandsArray[i];
		const char* commandName = commandJson["name"];
		CommandArgs* args = new CommandArgs();
		args->Duration = commandJson["duration"];
		args->Order = commandJson["order"];
		args->PinNumber = commandJson["pinNumber"];
		args->Value = commandJson["value"];	
		Command* command = _commandFactory->CreateCommand(commandName, args);

		Commands.push_back(command);		
	}

	std::sort(Commands.begin(), Commands.end(), Command::Compare);
}
