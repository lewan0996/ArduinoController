#include "IoTHubClient.h"

IoTHubClient* IoTHubClient::instance;

IoTHubClient::IoTHubClient(const String& connection_string, 
	std::pair<int, char*>(*handle_direct_method_callback)
	(const char *method_name, const char *payload, size_t payload_size))
{
	HandleDirectMethodCallback = handle_direct_method_callback;
	_connectionString = connection_string;
	Serial.println(_connectionString);
	instance = this; // solution to member function vs "normal" function pointer
}

int device_method_callback_non_member(const char * method_name, const unsigned char * payload, size_t size, unsigned char ** response, size_t * response_size, void * user_context_callback)
{
	return IoTHubClient::instance->device_method_callback(method_name, payload, size, response, response_size, user_context_callback);
}

void IoTHubClient::initialize()
{
	Serial.println("Initializing IoTHub client...");
	init_time();	
	
	_iotHubClientHandle = IoTHubClient_LL_CreateFromConnectionString(_connectionString.c_str(), MQTT_Protocol);
	if (_iotHubClientHandle == nullptr)
	{
		Serial.println("Failed to initialize IoT Hub client");
		return;
	}

	const IOTHUB_CLIENT_RESULT result = IoTHubClient_LL_SetDeviceMethodCallback(_iotHubClientHandle, device_method_callback_non_member, NULL);

	Serial.println(result);
	Serial.println("IoT hub initialized");
}

void IoTHubClient::do_work()
{
	IoTHubClient_LL_DoWork(_iotHubClientHandle);
}

int IoTHubClient::device_method_callback(const char * method_name, const unsigned char * payload, size_t payload_size, unsigned char ** response, size_t * response_size, void * user_context_callback)
{
	Serial.printf("Try to invoke method %s.\r\n", method_name);

	const std::pair<int, char*> result = HandleDirectMethodCallback(method_name, reinterpret_cast<const char*>(payload), payload_size);

	*response_size = strlen(result.second);
	*response = static_cast<unsigned char *>(malloc(*response_size));
	strncpy(reinterpret_cast<char *>(*response), result.second, *response_size);

	return result.first;
}

void IoTHubClient::init_time()
{
	configTime(0, 0, "pool.ntp.org", "time.nist.gov");

	while (true)
	{
		const auto epoch_time = time(nullptr);

		if (epoch_time == 0)
		{
			Serial.println("Fetching NTP epoch time failed! Waiting 2 seconds to retry.");
			delay(2000);
		}
		else
		{
			Serial.printf("Fetched NTP epoch time is: %lu.\r\n", epoch_time);
			break;
		}
	}
}
