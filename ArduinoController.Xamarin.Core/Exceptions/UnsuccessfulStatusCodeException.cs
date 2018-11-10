using System;

namespace ArduinoController.Xamarin.Core.Exceptions
{
    public class UnsuccessfulStatusCodeException : Exception
    {
        public UnsuccessfulStatusCodeException(string message): base(message)
        {
        }
    }
}
