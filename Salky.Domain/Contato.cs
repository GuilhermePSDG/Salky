using Salky.Domain.Salky.Domain;

namespace Salky.Domain
{
    public class Contato : BaseEntity
    {
        public Guid UserOwnerId { get; set; }
        public User UserOwner { get; set; }

        public Guid UserContactId { get; set; }
        public User UserContact { get; set; }

        public List<Message> Messages { get; set; } = new();

    }
}
