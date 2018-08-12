/*
	Name:       ArduinoController.ino
	Created:	30.06.2018 18:14:01
	Author:     MARCIN-LZ50-70\Marcin Lewandowski
*/

#include <Hash.h>
#include "IoTHubClient.h"
#include <ArduinoJson.hpp>
#include <ArduinoJson.h>
#include "ESP8266WiFi.h"
#include "Procedure.h"
#include "Command.h"
#include "StringHelpers.h";
#include "CommandFactory.h";

int accessPointStartTime;
int accessPointDuration = 10000;

bool isAccessPointInitializationDone;
CommandFactory* commandFactory = new CommandFactory();
IoTHubClient* ioTHubClient;

std::pair<int, char*> HandleDirectMethodCallback(const char* methodName, const char* payload, size_t payloadSize);
String GenerateIoTHubConnectionString(String macAddress);

void setup()
{
	Serial.begin(9600);

	String macAddress = WiFi.macAddress();

	String connectionString = GenerateIoTHubConnectionString(macAddress);

	Serial.println(connectionString);

	ioTHubClient = new IoTHubClient(connectionString, HandleDirectMethodCallback);

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

		if (accessPointDisabled)
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
	else if (isAccessPointInitializationDone)
	{
		ioTHubClient->DoWork();
		delay(10);
	}
}

String GenerateIoTHubConnectionString(String macAddress)
{
	String result = "HostName=arduino-controller-iot-hub.azure-devices.net;DeviceId=ESP_";
	result += macAddress;
	result += ";SharedAccessKey=";
	String hash = sha1(macAddress);
	hash.toUpperCase();
	result += hash;

	return result;
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

std::pair<int, char*> HandleDirectMethodCallback(const char* methodName, const char* payload, size_t payloadSize)
{
	int statusCode;
	char* responseMessage;
	if (strcmp(methodName, "ExecuteProcedure") == 0)
	{
		char* payloadValue = (char *)malloc(payloadSize + 1);
		strncpy(payloadValue, payload, payloadSize);
		payloadValue[payloadSize] = '\0';		

		bool result = HandleExecuteProcedureCall(payloadValue);

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
