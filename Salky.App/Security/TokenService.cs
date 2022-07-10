using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Salky.App.Interfaces;
using Salky.Domain.Models.UserModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Salky.App.Security
{
    public class TokenService : ITokenService
    {
        public static readonly TimeSpan TokenExpiresTime = TimeSpan.FromHours(24);
        public readonly SymmetricSecurityKey _key;

        public TokenService()
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTKEY") ?? throw new InvalidOperationException("Nenhum JWTKET token na variavel de ambiente")));
        }

        public async Task<AuthToken> CreateToken(User user)
        {
            return await Task.FromResult(this.CreateToken(ClaimsFromUser(user)));
          
        }
        public async Task<AuthToken> CreateToken(User User, List<Claim> Claims)
        {
            return await Task.FromResult(
                this.CreateToken(
                    this.ClaimsFromUser(User)
                    .Concat(Claims)
                    .ToList())
                );
        }
        public List<Claim> ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.ToList();
        }

        private AuthToken CreateToken(List<Claim> Claims)
        {
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var expireDate = DateTime.UtcNow.Add(TokenExpiresTime);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = expireDate,
                SigningCredentials = creds,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return new AuthToken(tokenHandler.WriteToken(token), expireDate);
        }

        private List<Claim> ClaimsFromUser(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
        }

    }
}
