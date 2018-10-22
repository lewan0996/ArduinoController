/*
	Name:       ArduinoController.ino
	Created:	30.06.2018 18:14:01
	Author:     MARCIN-LZ50-70\Marcin Lewandowski
*/

#include <Hash.h>
#include "IoTHubClient.h"
#include <ArduinoJson.hpp>
#include "ESP8266WiFi.h"
#include "Procedure.h"
#include "CommandFactory.h"

int access_point_start_time;
unsigned long access_point_duration = 10000;

bool is_access_point_initialization_done;
CommandFactory* command_factory = new CommandFactory();
IoTHubClient* iot_hub_client;

std::pair<int, char*> handle_direct_method_callback(const char* method_name, const char* payload, size_t payload_size);
String generate_iot_hub_connection_string(const String& mac_address);
void init_wifi();

void setup()
{
	Serial.begin(9600);

	const auto mac_address = WiFi.macAddress();

	auto connection_string = generate_iot_hub_connection_string(mac_address);

	Serial.println(connection_string);

	iot_hub_client = new IoTHubClient(connection_string, handle_direct_method_callback);

	String ssid = "ESP_" + mac_address;
	Serial.println("Enabling access point with SSID: " + ssid);
	const auto access_point_enabled = WiFi.softAP(ssid.c_str());
	if (access_point_enabled)
	{
		access_point_start_time = millis();
	}
	else
	{
		Serial.println("Enabling access point failed");
	}
}

void loop()
{
	if (!is_access_point_initialization_done && millis() - access_point_start_time > access_point_duration)
	{
		Serial.println("Disabling access point");
		const auto access_point_disabled = WiFi.softAPdisconnect();

		if (access_point_disabled)
		{
			Serial.println("Access point disabled");
		}
		else
		{
			Serial.println("Disabling access point failed");
		}

		is_access_point_initialization_done = true;
		init_wifi();

		iot_hub_client->Initialize();
	}
	else if (is_access_point_initialization_done)
	{
		iot_hub_client->DoWork();
		delay(10);
	}
}

String generate_iot_hub_connection_string(const String& mac_address)
{
	String result = "HostName=arduino-controller-iot-hub.azure-devices.net;DeviceId=";
	result += mac_address;
	result += ";SharedAccessKey=";
	auto hash = sha1(mac_address);
	hash.toUpperCase();
	result += hash;

	return result;
}

void init_wifi()
{
	// ReSharper disable once StringLiteralTypo
	const auto ssid = "dupsko";
	// ReSharper disable once StringLiteralTypo
	// ReSharper disable once CommentTypo
	const auto pass = "dupsko123"; // do zmiany

	Serial.printf("Attempting to connect to SSID: %s.\r\n", ssid);

	WiFi.begin(ssid, pass);
	while (WiFi.status() != WL_CONNECTED)
	{
		WiFi.begin(ssid, pass);
		delay(5000);
	}
	Serial.printf("Connected to wifi %s.\r\n", ssid);
}

bool handle_execute_procedure_call(const char* payload)
{
	auto procedure = new Procedure(command_factory);	
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
	delete procedure;
	return false;
}

std::pair<int, char*> handle_direct_method_callback(const char* method_name, const char* payload, size_t payload_size)
{
	int status_code;
	char const* response_message;
	if (strcmp(method_name, "ExecuteProcedure") == 0)
	{
		auto* payload_value = static_cast<char *>(malloc(payload_size + 1));
		strncpy(payload_value, payload, payload_size);
		payload_value[payload_size] = '\0';		

		const bool result = handle_execute_procedure_call(payload_value);

		if (result)
		{
			status_code = 200;
			response_message = "\"Method executed successfully\"";
		}
		else
		{
			status_code = 500;
			response_message = "\"An error occured while invoking the method\"";
		}
	}
	else
	{
		status_code = 404;
		response_message = "\"There is no such method\"";
	}

	return { status_code, const_cast<char*>(response_message) };
}
