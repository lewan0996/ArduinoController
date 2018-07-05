#pragma once
#include "vector";
#include "string.h";
class StringHelpers
{
public:
	StringHelpers();
	~StringHelpers();
	static std::vector<char*> Split(char* string, const char* delimeter);
};

