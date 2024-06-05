
using System.Net;
using System.Text.Json;
using Talabat.Errors;

namespace Talabat.Middlewares
{
    // By Convension
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger ,IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        // must be Name => InvokeAsync
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Take an Action


                await _next.Invoke(httpContext); // Go To The Next Middleware

                // Take an Action With The Response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message); //Development Env
                // log Exception in(DataBase | Files) // Prodaction Env

                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var response = _env.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    :
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}
