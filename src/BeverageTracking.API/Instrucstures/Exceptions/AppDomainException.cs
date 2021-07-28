using System;

namespace BeverageTracking.API.Instrucstures.Exceptions
{
    public class AppDomainException : Exception
    {
        public AppDomainException() { }

        public AppDomainException(string message) : base(message) { }
    }
}
