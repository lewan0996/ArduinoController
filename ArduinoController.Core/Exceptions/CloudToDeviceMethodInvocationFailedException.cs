using System;

namespace ArduinoController.Core.Exceptions
{
    public class CloudToDeviceMethodInvocationFailedException : Exception
    {
        public CloudToDeviceMethodInvocationFailedException(string message): base(message)
        {
            
        }
    }
}
