using Microsoft.AspNetCore.Http;
using Salky.App.Dtos.Users;
using System.Security.Claims;

namespace Salky.WebSocket.Infra.Interfaces
{
    public interface ISecurityProvider
    {
        public List<Claim> ValidateJwtToken(HttpContext context);
        public void DisconnectIfOnline(UserDto user);
    }
}
