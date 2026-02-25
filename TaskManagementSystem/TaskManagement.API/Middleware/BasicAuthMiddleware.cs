using System.Text;

namespace TaskManagement.API.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public BasicAuthMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant();
            if (path?.StartsWith("/swagger") == true || path?.StartsWith("/api/auth") == true)
            {
                await _next(context);
                return;
            }

            Console.WriteLine("=== REQUEST DEBUG ===");
            Console.WriteLine($"Path: {context.Request.Path}");

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault()?.Trim();
            Console.WriteLine($"Raw authHeader: '{authHeader ?? "NULL"}'");

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Basic Authorization header required");
                Console.WriteLine("Missing/Invalid auth header");
                return;
            }

            try
            {
                var base64Credentials = authHeader.Substring(6).Trim();
                Console.WriteLine($"Base64: '{base64Credentials}'");

                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(base64Credentials));
                Console.WriteLine($"Decoded: '{credentials}'");

                var parts = credentials.Split(':', 2);
                var username = parts[0].Trim();
                var password = parts[1].Trim();
                Console.WriteLine($"User: '{username}'");

                using var scope = _scopeFactory.CreateScope();
                var userRepository = scope.ServiceProvider.GetRequiredService<TaskManagement.Application.Common.Interfaces.IUserRepository>();

                var user = await userRepository.ValidateCredentialsAsync(username, password);
                if (user != null)
                {
                    context.Items["UserId"] = user.Id;
                    context.Items["Username"] = username;
                    Console.WriteLine("VALIDATED");
                    await _next(context);
                }
                else
                {
                    Console.WriteLine("DB Validation failed");
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid credentials");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Auth error");
            }
        }
    }
}