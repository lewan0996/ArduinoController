#include "Command.h"

Command::Command(CommandArgs* args)
{
	Order = args->Order;
}

bool Command::Compare(Command * x, Command * y)
{
	x->Order < y->Order;
}
