using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace CbsAp.API.Middlewares.Common
{
    [ExcludeFromCodeCoverage]
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var userName = context.User.Identity?.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                context.Items["UserName"] = userName;
            }
            await _next(context);
        }
    }
}
