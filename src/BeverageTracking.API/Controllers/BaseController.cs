using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeverageTracking.API.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
    }
}
