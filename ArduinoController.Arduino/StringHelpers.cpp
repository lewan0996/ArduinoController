#include "StringHelpers.h"



StringHelpers::StringHelpers()
= default;


StringHelpers::~StringHelpers()
= default;

std::vector<char*> StringHelpers::split(char* string, const char* delimiter)
{
	std::vector<char*> result;
	auto pch = strtok(string, delimiter);

	while (pch != nullptr)
	{
		result.push_back(pch);
		pch = strtok(nullptr, delimiter);
	}

	return result;
}