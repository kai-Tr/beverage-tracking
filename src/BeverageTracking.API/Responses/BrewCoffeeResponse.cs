using System;

namespace BeverageTracking.API
{
    public class BrewCoffeeResponse
    {

        public BrewCoffeeResponse(DateTime preparedTime, double temperature)
        {
            Prepared = preparedTime;
            _temperature = temperature;
        }

        private readonly double _temperature;
        public string Message
        {
            get { return _temperature > 30 ? "Your refreshing iced coffee is ready" : "Your piping hot coffee is ready"; }
        }

        public DateTime Prepared { get; private set; }
    }
}
