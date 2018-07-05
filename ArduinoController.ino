// Visual Micro is in vMicro>General>Tutorial Mode
// 
/*
	Name:       ArduinoController.ino
	Created:	30.06.2018 18:14:01
	Author:     MARCIN-LZ50-70\Marcin Lewandowski
*/

// Define User Types below here or use a .h file
//


// Define Function Prototypes that use User Types below here or use a .h file
//


// Define Functions below here or use other .ino or cpp files
//

// The setup() function runs once each time the micro-controller starts
#include "Command.h"
#include "StringHelpers.h";
#include "CommandFactory.h";

void setup()
{
	Serial.begin(9600);
	char* procedureString = "W_3000|DW_5_1|W_2000|AW_5_50|W_5000|N_5";
	std::vector<char*> commandStrings = StringHelpers::Split(procedureString, "|");
	std::vector<Command*> commands;

	for (int i = 0; i < commandStrings.size(); i++)
	{
		commands.push_back(CommandFactory::CreateCommand(commandStrings[i]));
	}

	for (int i = 0; i < commands.size(); i++)
	{
		commands[i]->Execute();
	}
}

// Add the main program code into the continuous loop() function
void loop()
{

}
