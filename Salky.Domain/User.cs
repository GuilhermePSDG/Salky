using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salky.Domain
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
        }
        public Guid Id { get; set; }
        public string? PictureSource { get; set; }
        public Visibility Visibility { get; set; }
        public List<Contato> FriendRequests
        {
            get => UserOwnerList.ToList();
        }
        public List<Contato> Contatos
        {
            get => UserContactList.ToList();
        }

        public IEnumerable<UserRole> UserRoles { get; set; }
        public List<Contato> UserOwnerList { get; set; } = new();
        public List<Contato> UserContactList { get; set; } = new();
    }
}
