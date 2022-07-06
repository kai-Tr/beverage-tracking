using BeverageTracking.API.Instrucstures.ActionResults;
using BeverageTracking.API.Instrucstures.Exceptions;
using BeverageTracking.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BeverageTracking.API.Controllers
{
    public class CoffeeController : BaseController
    {
        private readonly IBreakfastCoffeeService _coffeeService;
        private readonly ILogger _logger;
        public CoffeeController(IBreakfastCoffeeService coffeeService, ILoggerFactory loggerFactory)
        {
            _coffeeService = coffeeService;
            _logger = loggerFactory.CreateLogger("MyCategory");
        }

        [HttpGet("/brew-coffee")]
        [ProducesResponseType(typeof(BrewCoffeeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BrewCoffee()
        {
            try
            {
                _logger.LogInformation("Brew Coffee Action");
                return Ok();
            }
            catch (TeapotException)
            {
                return new TeapotErrorResult();
            }
            catch (ServiceUnavailableException)
            {
                return new ServiceUnavailableErrorResult();
            }
        }
    }
}
