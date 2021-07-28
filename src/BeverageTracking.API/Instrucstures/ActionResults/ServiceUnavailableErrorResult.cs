using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeverageTracking.API.Instrucstures.ActionResults
{
    public class ServiceUnavailableErrorResult : StatusCodeResult
    {
        public ServiceUnavailableErrorResult() : base(StatusCodes.Status503ServiceUnavailable)
        {
        }
    }
}
