using Microsoft.AspNetCore.Http;
using Salky.App.Interfaces;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Web;

namespace Salky.App.Security
{
    public class HttpWebSocketHandShaker : IConnectionAuthGuard
    {
        public ITokenService TokenService { get; }
        public HttpWebSocketHandShaker(ITokenService tokenService)
        {
            TokenService = tokenService;
        }
        public async Task<WebSocketUser?> AutorizeConnection(HttpContext httpContext)
        {
            var token = HttpUtility.ParseQueryString(httpContext.Request.QueryString.Value).Get("token");
            if (token == null) return null;
            var Claims = TokenService.ValidateToken(token);
            if (Claims == null || Claims.Count == 0) return null;
            var SocketKey = Claims.SingleOrDefault(f => f.Type.Equals("nameid"))?.Value;
            if (SocketKey == null) return null;
            return await Task.FromResult(new WebSocketUser(SocketKey, Claims));
        }
    }
}
