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

class iot_hub_client
{
public:
	iot_hub_client(const String& connection_string, std::pair<int, char*>(*handle_direct_method_callback)(const char* method_name, const char* payload, size_t payload_size));
	void initialize();
	void do_work();
	static iot_hub_client* instance;
	int device_method_callback(const char *method_name, const unsigned char *payload, size_t size, unsigned char **response, size_t *response_size, void *user_context_callback);
private:
	IOTHUB_CLIENT_LL_HANDLE iot_hub_client_handle_{};
	String connection_string_;
	std::pair<int, char*>(*handle_direct_method_callback_)(const char* methodName, const char* payload, size_t payloadSize);
	static void init_time();
};

#endif

