using System;

namespace BeverageTracking.API.Instrucstures.Exceptions
{
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException() { }

        public ServiceUnavailableException(string message) : base(message) { }
    }
}
