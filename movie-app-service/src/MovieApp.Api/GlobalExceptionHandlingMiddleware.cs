using Microsoft.AspNetCore.Mvc;
using static MovieApp.Core.Exceptions;

namespace MovieApp.Api
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled exception for request id: {context.TraceIdentifier}");
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    ValidationException => StatusCodes.Status400BadRequest,
                    ApplicationException => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Type = ex.GetType().Name,
                    Title = "Something weng wrong. Please contact support",
                    Detail = $"Unable to process request or request id :{context.TraceIdentifier}",
                });
            }
        }
    }
}
