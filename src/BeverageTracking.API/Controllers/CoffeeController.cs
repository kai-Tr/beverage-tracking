using BeverageTracking.API.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace BeverageTracking.API.Controllers
{
    public class CoffeeController : BaseController
    {
        private readonly IServerContext _serverContext;
        public CoffeeController(IServerContext serverContext)
        {
            _serverContext = serverContext;
        }

        [HttpGet("/brew-coffee")]
        [ProducesResponseType(typeof(BrewCoffeeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        public IActionResult BrewCoffee()
        {
            //on every fifth call to the endpoint,
            //the endpoint should return 503 Service Unavailable with an empty response body,
            //to signify that the coffee machine is out of coffee
            if (_serverContext.CoffeeStock == 0)
            {
                _serverContext.CoffeeStock = 4;
                return new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
            }

            //If the date is April 1st, then all calls to the endpoint defined in #1 should return 418 I’m a teapot 
            if (_serverContext.ServerTime.Month == 4 && _serverContext.ServerTime.Day == 1)
            {
                return new StatusCodeResult(StatusCodes.Status418ImATeapot);
            }

            _serverContext.CoffeeStock--;
            return Ok(new BrewCoffeeResponse(_serverContext.ServerTime));
        }
    }
}
