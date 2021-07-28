using System;

namespace BeverageTracking.API
{
    public class BrewCoffeeResponse
    {
        public BrewCoffeeResponse(DateTime preparedTime)
        {
            Prepared = preparedTime;
        }

        public string Message
        {
            get { return "Your piping hot coffee is ready"; }
        }

        public DateTime Prepared { get; private set; }
    }
}
