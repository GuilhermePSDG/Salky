using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket.Infra.Interfaces
{
    public interface HttpWebSocketGuard
    {
        public bool CanContinue(HttpContext httpContext,List<Claim>? Claims,string? Key);
    }
    public interface HttpWebSocketGuardIdentityProvider
    {
        public bool CanContinue(HttpContext httpContext, [NotNullWhen(true)] out  List<Claim>? Claims,[NotNullWhen(true)]out string? SocketKey);
    }

}
