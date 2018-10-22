#include "Command.h"

command::command(command_args* args)
{
	order = args->order;
}

bool command::compare(command * x, command * y)
{
	return x->order < y->order;
}
