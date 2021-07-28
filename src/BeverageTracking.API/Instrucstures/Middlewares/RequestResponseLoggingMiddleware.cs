using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageTracking.API.Instrucstures.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation(await FormatRequest(context.Request));
            await _next(context);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var bodyAsText = string.Empty;
            if (request.ContentLength.HasValue)
            {
                using var reader = new StreamReader(
                   request.Body,
                   encoding: Encoding.UTF8,
                   detectEncodingFromByteOrderMarks: false,
                   bufferSize: 1024 * 32,
                   leaveOpen: true);
                bodyAsText = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }
    }
}
