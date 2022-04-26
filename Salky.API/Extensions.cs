using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salky.WebSocket.Handler;
using System.Security.Claims;

namespace Salky.API
{
  
    public static class Extensions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            var value =  user.FindFirst(ClaimTypes.Name)?.Value;
            return value ?? throw new Exception("Cannot extract UserName from token");
        }

        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(value ?? throw new Exception("Cannot extract Id from token"));
        }

        public static IApplicationBuilder UseSalkyWebSocket(this IApplicationBuilder app)
        {
            app.UseWebSockets();
            return app.UseMiddleware<WebServerSocketMiddleware>();
        }
    }
}
