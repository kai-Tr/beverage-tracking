using BeverageTracking.API.Instrucstures.ActionResults;
using BeverageTracking.API.Instrucstures.Exceptions;
using BeverageTracking.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeverageTracking.API.Controllers
{
    public class CoffeeController : BaseController
    {
        private readonly ICoffeeService _coffeeService;
        public CoffeeController(ICoffeeService coffeeService)
        {
            _coffeeService = coffeeService;
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
                return Ok(await _coffeeService.BrewAsync());
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
