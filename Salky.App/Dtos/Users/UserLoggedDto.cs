namespace Salky.App.Dtos.Users
{
    public class UserLoggedDto : UserMinimalDto
    {
        public string Token { get; set; }
        public DateTime TokenExpire { get; set; }
    }
}
