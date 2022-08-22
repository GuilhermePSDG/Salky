using Microsoft.AspNetCore.Http;
using Salky.App.Interfaces;
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
        public async Task<WebSocketUser?> AuthenticateConnection(HttpContext httpContext)
        {
            var token = TokenFromQueryString(httpContext.Request.QueryString.Value);
            if (string.IsNullOrEmpty(token)) return null;
            var Claims = TokenService.ValidateToken(token);
            if (Claims == null || Claims.Count == 0) return null;
            var SocketKey = Claims.SingleOrDefault(f => f.Type.Equals("nameid"))?.Value;
            if (SocketKey == null) return null;
            return await Task.FromResult(new WebSocketUser(SocketKey, Claims));
        }

        private string? TokenFromQueryString(string? value, string queryKey = "token")
        {
            return value == null ? null : HttpUtility.ParseQueryString(value).Get(queryKey);
        }

        private string? TokenFromHeaders(IHeaderDictionary headers)
        {
            if (!headers.TryGetValue("sec-websocket-protocol", out var protocol))
                return null;
            var protocols = protocol.ToString().Split(", ");
            return protocols[1];
        }

    }
}
