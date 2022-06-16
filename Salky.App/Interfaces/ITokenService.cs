using Salky.Domain.Models.UserModels;
using System.Security.Claims;

namespace Salky.App.Interfaces
{
    public record AuthToken(string value,DateTime expires);
    public interface ITokenService
    {
        Task<AuthToken> CreateToken(User User);
        Task<AuthToken> CreateToken(User User,List<Claim> Claims);
        public List<Claim>? ValidateToken(string Token);
    }
}
