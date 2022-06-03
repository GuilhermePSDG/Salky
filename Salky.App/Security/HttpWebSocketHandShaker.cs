using Microsoft.AspNetCore.Http;
using Salky.App.Interfaces;
using Salky.WebSocket.Infra.Interfaces;
using System.Security.Claims;
using System.Web;

namespace Salky.App.Security
{
    public class HttpWebSocketHandShaker : IDoHttpHandshake
    {
        public ITokenService TokenService { get; }
        public HttpWebSocketHandShaker(ITokenService tokenService)
        {
            TokenService = tokenService;
        }
        public void MakeOrThrow(HttpContext httpContext, out List<Claim> Claims, out string SocketKey)
        {
            var token = HttpUtility.ParseQueryString(httpContext.Request.QueryString.Value).Get("token") ?? throw new InvalidOperationException("Token not found");
            var claims = TokenService.ValidateToken(token);
            if (claims == null || claims.Count == 0) throw new NullReferenceException("Invalid Token");
            var key = claims.SingleOrDefault(f => f.Type.Equals("nameid")) ?? throw new NullReferenceException("Invalid Token");
            SocketKey = key.Value;
            Claims = claims;
        }
    }
}
