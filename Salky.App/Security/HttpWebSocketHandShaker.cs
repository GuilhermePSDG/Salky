using Microsoft.AspNetCore.Http;
using Salky.App.Interfaces;
using Salky.WebSocket.Infra.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Web;

namespace Salky.App.Security
{
    public class HttpWebSocketHandShaker : HttpWebSocketGuardIdentityProvider
    {
        public ITokenService TokenService { get; }
        public HttpWebSocketHandShaker(ITokenService tokenService)
        {
            TokenService = tokenService;
        }
        public bool CanContinue(HttpContext httpContext, [NotNullWhen(true)] out List<Claim>? Claims, [NotNullWhen(true)] out string? SocketKey)
        {
            Claims = null;
            SocketKey = null;
            var token = HttpUtility.ParseQueryString(httpContext.Request.QueryString.Value).Get("token");
            if (token == null) return false;
            Claims = TokenService.ValidateToken(token);
            if (Claims == null || Claims.Count == 0) return false;
            SocketKey = Claims.SingleOrDefault(f => f.Type.Equals("nameid"))?.Value;
            if(SocketKey == null) return false;
            return true;
        }
    }
}
