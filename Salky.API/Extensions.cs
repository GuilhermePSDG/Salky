using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Salky.API
{
  
    public static class Extensions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            var value =  user.FindFirst(ClaimTypes.Name)?.Value;
            return value ?? throw new Exception("Cannot extract UserName from token");
        }
        public static string GetUserName(this IEnumerable<Claim> claims)
        {
            var value = claims.FirstOrDefault(f => f.Type == "unique_name")?.Value;
            return value ?? throw new Exception("Cannot extract username from token");
        }
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(value ?? throw new Exception("Cannot extract Id from token"));
        }

        public static Guid GetUserId(this IEnumerable<Claim> claims)
        {
            var value = claims.FirstOrDefault(f => f.Type == "nameid")?.Value;
            return Guid.Parse(value ?? throw new Exception("Cannot extract Id from token"));
        }

      
    }
}
