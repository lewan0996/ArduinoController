using System;
using System.Net;

namespace ArduinoController.Xamarin.Core.Exceptions
{
    public class UnsuccessfulStatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorPhrase { get; set; }

        public UnsuccessfulStatusCodeException(string message, HttpStatusCode statusCode, string errorPhrase) : base(message)
        {
            StatusCode = statusCode;
            ErrorPhrase = errorPhrase;
        }
    }
}
