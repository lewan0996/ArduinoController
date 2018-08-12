#include "IoTHubClient.h"

IoTHubClient* IoTHubClient::Instance;

IoTHubClient::IoTHubClient(String connectionString, std::pair<int, char*>(*handleDirectMethodCallback)(const char *methodName, const char *payload, size_t payloadSize))
{
	HandleDirectMethodCallback = handleDirectMethodCallback;
	_connectionString = connectionString;
	Serial.println(_connectionString);
	IoTHubClient::Instance = this; // solution to member function vs "normal" function pointer
}

int DeviceMethodCallback_NonMember(const char * methodName, const unsigned char * payload, size_t size, unsigned char ** response, size_t * response_size, void * userContextCallback)
{
	return IoTHubClient::Instance->DeviceMethodCallback(methodName, payload, size, response, response_size, userContextCallback);
}

void IoTHubClient::Initialize()
{
	Serial.println("Initializing IoTHub client...");
	InitTime();	
	
	_iotHubClientHandle = IoTHubClient_LL_CreateFromConnectionString(_connectionString.c_str(), MQTT_Protocol);
	if (_iotHubClientHandle == NULL)
	{
		Serial.println("Failed to initalize IoT Hub client");
		return;
	}
	
	IOTHUB_CLIENT_RESULT result = IoTHubClient_LL_SetDeviceMethodCallback(_iotHubClientHandle, DeviceMethodCallback_NonMember, NULL);

	Serial.println(result);
	Serial.println("IoT hub initialized");
}

void IoTHubClient::DoWork()
{
	IoTHubClient_LL_DoWork(_iotHubClientHandle);
}

int IoTHubClient::DeviceMethodCallback(const char * methodName, const unsigned char * payload, size_t payloadSize, unsigned char ** response, size_t * response_size, void * userContextCallback)
{
	Serial.printf("Try to invoke method %s.\r\n", methodName);		

	std::pair<int, char*> result = HandleDirectMethodCallback(methodName, reinterpret_cast<const char*>(payload), payloadSize);

	*response_size = strlen(result.second);
	*response = (unsigned char *)malloc(*response_size);
	strncpy((char *)(*response), result.second, *response_size);

	return result.first;
}

void IoTHubClient::InitTime()
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
