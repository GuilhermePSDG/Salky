using Salky.App.Dtos.Users;

namespace Salky.App.Dtos.Auth
{
    public class UserLoggedDto : UserDto
    {
        public string Token { get; set; }
    }
}
