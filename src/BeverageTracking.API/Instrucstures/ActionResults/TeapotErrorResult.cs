using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeverageTracking.API.Instrucstures.ActionResults
{
    public class TeapotErrorResult : StatusCodeResult
    {
        public TeapotErrorResult() : base(StatusCodes.Status418ImATeapot)
        {
        }
    }
}
