using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Salky.WebSocket.Infra.Interfaces
{
    public interface IValidateToken
    {
        /// <summary>
        /// Keys is used for recovery the WebSocket Connection
        /// <para></para>
        /// Key must be unique
        /// </summary>
        /// <param name="token"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public List<Claim> ValidateJwtTokenOrThrow(string token, out string Key);
    }
}
