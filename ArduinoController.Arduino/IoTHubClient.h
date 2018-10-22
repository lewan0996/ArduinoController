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
	IoTHubClient(const String& connectionString, std::pair<int, char*>(*handleDirectMethodCallback)(const char* methodName, const char* payload, size_t payloadSize));
	void initialize();
	void do_work();
	static IoTHubClient* instance;
	int device_method_callback(const char *methodName, const unsigned char *payload, size_t size, unsigned char **response, size_t *response_size, void *userContextCallback);
private:
	IOTHUB_CLIENT_LL_HANDLE _iotHubClientHandle{};
	String _connectionString;
	std::pair<int, char*>(*HandleDirectMethodCallback)(const char* methodName, const char* payload, size_t payloadSize);
	static void init_time();
};

#endif

