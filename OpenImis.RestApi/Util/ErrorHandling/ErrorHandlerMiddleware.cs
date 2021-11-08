using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace OpenImis.RestApi.Util.ErrorHandling
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _request;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;
        public ErrorHandlerMiddleware(RequestDelegate request, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            _logger = loggerFactory.CreateLogger<ErrorHandlerMiddleware>();
            _request = request;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _request(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (exception is BusinessException businessException)
            {
                context.Response.StatusCode = (int)businessException.Status;
                await context.Response.WriteAsync(GetErrorResponse(businessException.Message));
            } 
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                if (_env.IsDevelopment())
                {
                    await context.Response.WriteAsync(GetErrorResponse(exception.Message));
                }
                else
                {
                    await context.Response.WriteAsync(GetErrorResponse("Internal Server Error"));
                }
            }
        }

        private string GetErrorResponse(string message)
        {
            return JsonConvert.SerializeObject(new { error_occured = true, error_message = message });
        }
    }
}
