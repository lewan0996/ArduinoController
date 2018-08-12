// IoTHubClient.h

#ifndef _IOTHUBCLIENT_h
#define _IOTHUBCLIENT_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif

#include <AzureIoTHub.h>
#include <AzureIoTProtocol_MQTT.h>
#include <AzureIoTUtility.h>

class IoTHubClient 
{
public:
	IoTHubClient(const char* connectionString, std::pair<int,char*>(*handleDirectMethodCallback)(const char* methodName, const char* payload));
	void Initialize();
	void DoWork();
private:
	IOTHUB_CLIENT_LL_HANDLE _iotHubClientHandle;
	const char* _connectionString;
	std::pair<int, char*>(*HandleDirectMethodCallback)(const char* methodName, const char* payload);
	int DeviceMethodCallback(const char *methodName, const unsigned char *payload, size_t size, unsigned char **response, size_t *response_size, void *userContextCallback);
	void InitTime();
};

#endif

