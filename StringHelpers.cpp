#include "StringHelpers.h"



StringHelpers::StringHelpers()
{
}


StringHelpers::~StringHelpers()
{
}

std::vector<char*> StringHelpers::Split(char* string, const char* delimeter)
{
	std::vector<char*> result;
	char* pch;
	pch = strtok(string, delimeter);

	while (pch != NULL)
	{
		result.push_back(pch);
		pch = strtok(NULL, delimeter);
	}

	return result;
}
