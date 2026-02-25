using System.Text.Json;

namespace TaskManagement.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Global exception: {Message}\nStackTrace: {StackTrace}\nPath: {Path}",
                    ex.Message, ex.StackTrace, context.Request.Path);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    message = "Internal server error",
                    timestamp = DateTime.UtcNow,
                    path = context.Request.Path,
                    method = context.Request.Method
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}