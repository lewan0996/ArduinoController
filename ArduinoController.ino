/*
	Name:       ArduinoController.ino
	Created:	30.06.2018 18:14:01
	Author:     MARCIN-LZ50-70\Marcin Lewandowski
*/

#include "ESP8266WiFi.h"
#include "Procedure.h"
#include "Command.h"
#include "StringHelpers.h";
#include "CommandFactory.h";

int accessPointStartTime;
int accessPointDuration = 30000;
bool isAccessPointInitializationDone;
CommandFactory* commandFactory = new CommandFactory();

void setup()
{
	Serial.begin(9600);	
	
	String macAddress = WiFi.macAddress();
	String SSID = "ESP_" + macAddress;
	Serial.println("Enabling access point with SSID: " + SSID);
	bool accessPointEnabled = WiFi.softAP(SSID.c_str());
	if (accessPointEnabled)
	{
		accessPointStartTime = millis();
	}
	else
	{
		Serial.println("Enabling access point failed");
	}	
}

void HandleMessage(char* message) 
{
	Procedure* procedure = new Procedure(commandFactory);

	Serial.println("Parsing json...");
	procedure->LoadJson(message);
	if (procedure->isValid)
	{
		Serial.println("Parsing done...");
		Serial.println("Executing procedure...");
		procedure->Execute();
		Serial.println("Procedure executed");
	}

	delete procedure;
}

void loop()
{
	if (millis() - accessPointStartTime > accessPointDuration && !isAccessPointInitializationDone)
	{
		Serial.println("Disabling access point");		
		bool accessPointDisabled = WiFi.softAPdisconnect();

		if(accessPointDisabled)
		{
			Serial.println("Access point disabled");
		}
		else
		{
			Serial.println("Disabling access point failed");
		}

		isAccessPointInitializationDone = true;
	}
}
