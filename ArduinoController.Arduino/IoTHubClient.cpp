#include "IoTHubClient.h"

iot_hub_client* iot_hub_client::instance;

iot_hub_client::iot_hub_client(const String& connection_string, 
	std::pair<int, char*>(*handle_direct_method_callback)
	(const char *method_name, const char *payload, size_t payload_size))
{
	handle_direct_method_callback_ = handle_direct_method_callback;
	connection_string_ = connection_string;
	Serial.println(connection_string_);
	instance = this; // solution to member function vs "normal" function pointer
}

int device_method_callback_non_member(const char * method_name, const unsigned char * payload, size_t size, unsigned char ** response, size_t * response_size, void * user_context_callback)
{
	return iot_hub_client::instance->device_method_callback(method_name, payload, size, response, response_size, user_context_callback);
}

void iot_hub_client::initialize()
{
	Serial.println("Initializing IoTHub client...");
	init_time();	
	
	iot_hub_client_handle_ = IoTHubClient_LL_CreateFromConnectionString(connection_string_.c_str(), MQTT_Protocol);
	if (iot_hub_client_handle_ == nullptr)
	{
		Serial.println("Failed to initialize IoT Hub client");
		return;
	}

	const IOTHUB_CLIENT_RESULT result = IoTHubClient_LL_SetDeviceMethodCallback(iot_hub_client_handle_, device_method_callback_non_member, NULL);

	Serial.println(result);
	Serial.println("IoT hub initialized");
}

void iot_hub_client::do_work()
{
	IoTHubClient_LL_DoWork(iot_hub_client_handle_);
}

int iot_hub_client::device_method_callback(const char * method_name, const unsigned char * payload, size_t payload_size, unsigned char ** response, size_t * response_size, void * user_context_callback)
{
	Serial.printf("Try to invoke method %s.\r\n", method_name);

	const std::pair<int, char*> result = handle_direct_method_callback_(method_name, reinterpret_cast<const char*>(payload), payload_size);

	*response_size = strlen(result.second);
	*response = static_cast<unsigned char *>(malloc(*response_size));
	strncpy(reinterpret_cast<char *>(*response), result.second, *response_size);

	return result.first;
}

void iot_hub_client::init_time()
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
