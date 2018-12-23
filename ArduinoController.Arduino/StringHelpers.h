#pragma once
#include "vector";
#include "string.h";
class string_helpers
{
public:
	string_helpers();	
	static std::vector<char*> split(char* string, const char* delimiter);	
};

struct compare_c_strings
{
	bool operator()(const char* a, const char* b) const {
		return strcmp(a, b) < 0;
	}
};

