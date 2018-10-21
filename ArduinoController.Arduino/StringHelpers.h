#pragma once
#include "vector";
#include "string.h";
class StringHelpers
{
public:
	StringHelpers();
	~StringHelpers();
	static std::vector<char*> split(char* string, const char* delimeter);	
};

struct CompareCStrings
{
	bool operator()(const char* a, const char* b) const {
		return strcmp(a, b) < 0;
	}
};

