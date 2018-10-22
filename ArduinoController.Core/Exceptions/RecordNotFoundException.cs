using System;

namespace ArduinoController.Core.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(string message = null) : base(message)
        {
        }
    }
}
