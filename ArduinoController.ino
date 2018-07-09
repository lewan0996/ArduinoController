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
#include "Procedure.h"
#include "Command.h"
#include "StringHelpers.h";
#include "CommandFactory.h";

void setup()
{
	Serial.begin(9600);
	char* procedureJson = "[{\"name\":\"DigitalWrite\",\"pinNumber\":5, \"value\":1, \"order\":1},{\"name\":\"Wait\", \"duration\":5000,\"order\":0}]";
	CommandFactory* commandFactory = new CommandFactory();
	Procedure* procedure = new Procedure(commandFactory);

	Serial.println("Parsing json...");
	procedure->LoadJson(procedureJson);
	Serial.println("Parsing done...");
	Serial.println("Executing procedure...");
	procedure->Execute();
	Serial.println("Procedure executed");
	
}

// Add the main program code into the continuous loop() function
void loop()
{

}
