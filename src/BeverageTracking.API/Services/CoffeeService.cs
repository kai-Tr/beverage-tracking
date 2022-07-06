using BeverageTracking.API.Connectors;
using BeverageTracking.API.Context;
using BeverageTracking.API.Instrucstures.Exceptions;
using BeverageTracking.API.Repositories;
using System.Threading.Tasks;

namespace BeverageTracking.API.Services
{
    public class CoffeeService : ICoffeeService
    {
        private readonly IWeatherConnector _weatherConnector;
        private readonly IServerContext _serverContext;
        private readonly ICoffeeStockRepository _coffeeStockRepository;

        public CoffeeService(IWeatherConnector weatherConnector, IServerContext serverContext, ICoffeeStockRepository coffeeStockRepository)
        {
            _weatherConnector = weatherConnector;
            _serverContext = serverContext;
            _coffeeStockRepository = coffeeStockRepository;
        }

        public virtual async Task<BrewCoffeeResponse> BrewAsync()
        {
            if (_serverContext.ServerTime.Month == 4 && _serverContext.ServerTime.Day == 1)
            {
                throw new TeapotException("I’m a teapot");
            }

            var temperarture = await _weatherConnector.GetTemperatureAsync();
            _coffeeStockRepository.RemoveStock(1);
            return new BrewCoffeeResponse(_serverContext.ServerTime, temperarture);
        }
    }

    public class BreakfastCoffeeService : CoffeeService
    {
        public BreakfastCoffeeService(IWeatherConnector weatherConnector, IServerContext serverContext, ICoffeeStockRepository coffeeStockRepository)
            : base(weatherConnector, serverContext, coffeeStockRepository)
        {

        }

        public void Test()
        {

        }
    }
}
