using System;

namespace BeverageTracking.API.Instrucstures.Exceptions
{
    public class TeapotException : Exception
    {
        public TeapotException() { }

        public TeapotException(string message) : base(message) { }
    }
}
