using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using System.Threading.Tasks;

namespace WebApiAgenda.Middleware
{

    public class DengoMiddleware
    {
        private readonly RequestDelegate _next;

        public DengoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

    
    public static class DengoMiddlewareExtensions
    {
        public static IApplicationBuilder UseDengoMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DengoMiddleware>();
        }
    }
}
