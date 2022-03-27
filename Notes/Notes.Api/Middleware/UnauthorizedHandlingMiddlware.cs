using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace Notes.Api.Middleware
{
    public class UnauthorizedHandlingMiddlware
    {
        private readonly RequestDelegate next;

        public UnauthorizedHandlingMiddlware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await next?.Invoke(context);

            if (IsUnauthorized(context))
            {
                await HandleUnauthorizedResponse(context);
            }
        }

        private static async Task HandleUnauthorizedResponse(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Пользователь не авторизован в системе.");
        }

        private static bool IsUnauthorized(HttpContext context)
        {
            return context.Response.StatusCode == (int)HttpStatusCode.Unauthorized;
        }
    }
}
