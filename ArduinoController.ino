/*
	Name:       ArduinoController.ino
	Created:	30.06.2018 18:14:01
	Author:     MARCIN-LZ50-70\Marcin Lewandowski
*/

#include <ArduinoJson.hpp>
#include <ArduinoJson.h>
#include "ESP8266WiFi.h"
#include "Procedure.h"
#include "Command.h"
#include "StringHelpers.h";
#include "CommandFactory.h";
#include <AzureIoTHub.h>
#include <AzureIoTProtocol_MQTT.h>
#include <AzureIoTUtility.h>

int accessPointStartTime;
int accessPointDuration = 30000;
const char* ioTHubconnectionString = "HostName=arduino-controller-iot-hub.azure-devices.net;DeviceId=ESP_A0:20:A6:01:07:C0;SharedAccessKey=LlysjPcI/1B/VXVdJPN/YhaCMlPpUdoCH9aONlfsRZ0=";
bool isAccessPointInitializationDone;
CommandFactory* commandFactory = new CommandFactory();
IOTHUB_CLIENT_LL_HANDLE iotHubClientHandle;

void setup()
{
	Serial.begin(9600);

	initWifi();
	initTime();

	SetupIoTHubClient();

	/*String macAddress = WiFi.macAddress();
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
	}*/
}

void SetupIoTHubClient()
{
	iotHubClientHandle = IoTHubClient_LL_CreateFromConnectionString(ioTHubconnectionString, MQTT_Protocol);
	if (iotHubClientHandle == NULL)
	{
		Serial.println("Failed to initalize IoT Hub client");
	}
	//IOTHUB_CLIENT_RESULT result = IoTHubClient_LL_SetMessageCallback(iotHubClientHandle, receiveMessageCallback, NULL);
	IOTHUB_CLIENT_RESULT result = IoTHubClient_LL_SetDeviceMethodCallback(iotHubClientHandle, deviceMethodCallback, NULL);

	Serial.println(result);

	Serial.println("IoT hub initialized");
}

IOTHUBMESSAGE_DISPOSITION_RESULT receiveMessageCallback(IOTHUB_MESSAGE_HANDLE message, void *userContextCallback)
{
	IOTHUBMESSAGE_DISPOSITION_RESULT result;
	const unsigned char *buffer;
	size_t size;
	if (IoTHubMessage_GetByteArray(message, &buffer, &size) != IOTHUB_MESSAGE_OK)
	{
		Serial.println("Unable to IoTHubMessage_GetByteArray.");
		result = IOTHUBMESSAGE_REJECTED;
	}
	else
	{
		/*buffer is not zero terminated*/
		char *temp = (char *)malloc(size + 1);

		if (temp == NULL)
		{
			return IOTHUBMESSAGE_ABANDONED;
		}

		strncpy(temp, (const char *)buffer, size);
		temp[size] = '\0';
		Serial.println(temp);
		if (!HandleMessage(temp))
		{
			free(temp);
			return IOTHUBMESSAGE_ABANDONED;			
		}
		free(temp);
	}
	return IOTHUBMESSAGE_ACCEPTED;
}

void initWifi()
{
	const char* ssid = "dupsko";
	const char* pass = "dupsko123";
	// Attempt to connect to Wifi network:
	Serial.printf("Attempting to connect to SSID: %s.\r\n", ssid);

	// Connect to WPA/WPA2 network. Change this line if using open or WEP network:
	WiFi.begin(ssid, pass);
	while (WiFi.status() != WL_CONNECTED)
	{
		WiFi.begin(ssid, pass);
		delay(5000);
	}
	Serial.printf("Connected to wifi %s.\r\n", ssid);
}

bool HandleMessage(const char* message)
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

		delete procedure;
		return true;
	}
	else 
	{
		delete procedure;
		return false;
	}	
}

void initTime()
{
	time_t epochTime;
	configTime(0, 0, "pool.ntp.org", "time.nist.gov");

	while (true)
	{
		epochTime = time(NULL);

		if (epochTime == 0)
		{
			Serial.println("Fetching NTP epoch time failed! Waiting 2 seconds to retry.");
			delay(2000);
		}
		else
		{
			Serial.printf("Fetched NTP epoch time is: %lu.\r\n", epochTime);
			break;
		}
	}
}

int deviceMethodCallback(const char *methodName, const unsigned char *payload, size_t size, unsigned char **response, size_t *response_size, void *userContextCallback)
{
	Serial.printf("Try to invoke method %s.\r\n", methodName);
	const char *responseMessage = "success";
	int result = 200;		

	*response_size = strlen(responseMessage);
	*response = (unsigned char *)malloc(*response_size);
	strncpy((char *)(*response), responseMessage, *response_size);

	return result;
}

void loop()
{
	IoTHubClient_LL_DoWork(iotHubClientHandle);
	delay(1000);
	/*if (millis() - accessPointStartTime > accessPointDuration && !isAccessPointInitializationDone)
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
		SetupIoTHubClient();
	}*/
}
