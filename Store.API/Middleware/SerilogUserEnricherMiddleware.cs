using Serilog.Context;
using System.Security.Claims;

namespace Store.API.Middleware
{
    public class SerilogUserEnricherMiddleware
    {
        private readonly RequestDelegate _next;

        public SerilogUserEnricherMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userEmail = context.User?.FindFirstValue(ClaimTypes.Email) ?? "Guest";
            
            using (LogContext.PushProperty("UserEmail", userEmail))
            {
                await _next(context);
            }
        }
    }
}
