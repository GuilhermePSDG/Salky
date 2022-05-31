using Salky.Domain.Models.UserModels;
using System.Security.Claims;

namespace Salky.App.Interfaces
{
    public record Token(string value,DateTime expires);
    public interface ITokenService
    {
        Task<Token> CreateToken(User User);
        Task<Token> CreateToken(User User,List<Claim> Claims);
        public List<Claim>? ValidateToken(string Token);
    }
}
