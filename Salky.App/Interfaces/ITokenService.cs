using Salky.Domain;
using System.Security.Claims;

namespace Salky.App.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User userUpdateDto);

        public List<Claim>? ValidateToken(string token);
    }
}
