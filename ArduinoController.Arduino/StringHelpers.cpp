#include "StringHelpers.h"

string_helpers::string_helpers()
= default;

std::vector<char*> string_helpers::split(char* string, const char* delimiter)
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