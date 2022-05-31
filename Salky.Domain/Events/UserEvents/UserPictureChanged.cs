using Salky.Domain.Contracts;
using Salky.Domain.Models.UserModels;

namespace Salky.Domain.Events.UserEvents
{
    public class UserPictureChanged : FieldChanged<string>, IDomainEvent
    {
        public UserPictureChanged(Guid Id, string NewPicture) : base(Id, NewPicture, nameof(User.PictureSource))
        {

        }
    }
}