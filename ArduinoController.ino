/*
	Name:       ArduinoController.ino
	Created:	30.06.2018 18:14:01
	Author:     MARCIN-LZ50-70\Marcin Lewandowski
*/

#include "IoTHubClient.h"
#include <ArduinoJson.hpp>
#include <ArduinoJson.h>
#include "ESP8266WiFi.h"
#include "Procedure.h"
#include "Command.h"
#include "StringHelpers.h";
#include "CommandFactory.h";

int accessPointStartTime;
int accessPointDuration = 30000;
const char* ioTHubconnectionString = "HostName=arduino-controller-iot-hub.azure-devices.net;DeviceId=ESP_A0:20:A6:01:07:C0;SharedAccessKey=LlysjPcI/1B/VXVdJPN/YhaCMlPpUdoCH9aONlfsRZ0=";
bool isAccessPointInitializationDone;
CommandFactory* commandFactory = new CommandFactory();
IoTHubClient* ioTHubClient;

std::pair<int, char*> HandleDirectMethodCallback(const char* methodName, const char* payload);
const char* GenerateIoTHubConnectionString(const char* macAddress);


void setup()
{
	Serial.begin(9600);

	initWifi();	

	String macAddress = WiFi.macAddress();

	ioTHubClient = new IoTHubClient(GenerateIoTHubConnectionString(macAddress.c_str()), HandleDirectMethodCallback);

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

void loop()
{		
	if (!isAccessPointInitializationDone && millis() - accessPointStartTime > accessPointDuration)
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
		initWifi();

		ioTHubClient->Initialize();
	}
	else
	{
		ioTHubClient->DoWork();
		delay(10);
	}
}

const char* GenerateIoTHubConnectionString(const char* macAddress)
{
	char* result;
	strcpy(result, "HostName=arduino-controller-iot-hub.azure-devices.net;DeviceId=ESP_");
	strcpy(result, macAddress);
	strcpy(result, "SharedAccessKey=");
	strcpy(result, "LlysjPcI/1B/VXVdJPN/YhaCMlPpUdoCH9aONlfsRZ0=");
}

void initWifi()
{
	const char* ssid = "dupsko";
	const char* pass = "dupsko123"; // do zmiany

	Serial.printf("Attempting to connect to SSID: %s.\r\n", ssid);

	WiFi.begin(ssid, pass);
	while (WiFi.status() != WL_CONNECTED)
	{
		WiFi.begin(ssid, pass);
		delay(5000);
	}
	Serial.printf("Connected to wifi %s.\r\n", ssid);
}

bool HandleExecuteProcedureCall(const char* payload)
{
	Procedure* procedure = new Procedure(commandFactory);

	Serial.println("Parsing json...");
	procedure->LoadJson(payload);
	if (procedure->isValid)
	{
		Serial.println("Parsing done...");
		Serial.println("Executing procedure...");
		procedure->Execute();
		Serial.println("Procedure executed");

		delete procedure;
		return true;
	}
	else
	{
		delete procedure;
		return false;
	}
}

std::pair<int, char*> HandleDirectMethodCallback(const char* methodName, const char* payload)
{
	int statusCode;
	char* responseMessage;
	if (strcmp(methodName, "ExecuteProcedure"))
	{
		bool result = HandleExecuteProcedureCall(reinterpret_cast<const char*>(payload));

		if (result)
		{
			statusCode = 200;
			responseMessage = "\"Method executed successfully\"";
		}
		else
		{
			statusCode = 500;
			responseMessage = "\"An error occured while invoking the method\"";
		}
	}
	else
	{
		statusCode = 404;
		responseMessage = "\"There is no such method\"";
	}

	return std::pair<int, char*>(statusCode, responseMessage);
}
