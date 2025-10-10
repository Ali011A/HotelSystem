
using Hotel.Application.Common.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Api.Middleware
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
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
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";
            int statusCode;
            string title;
            string detail;

            switch (exception)
            {
                case NotFoundException notFound:
                    statusCode = StatusCodes.Status404NotFound;
                    title = "Resource Not Found";
                    detail = notFound.Message;
                    break;
                case Hotel.Application.Common.Exceptions.ValidationException validation:
                    statusCode = StatusCodes.Status400BadRequest;
                    title = "Validation Error";
                    detail = validation.Message;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    title = "An unexpected error occurred.";
                    detail = "An internal server error has occurred. Please try again later.";
                    break;
            }

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(new { Title = title, Detail = detail });
        }
    }
}
