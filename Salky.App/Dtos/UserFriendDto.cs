using Salky.App.Dtos.Users;
using Salky.Domain.Models.FriendModels;

namespace Salky.App.Dtos
{
    public class UserFriendDto : UserMinimalDto
    {
        public bool RequestByCurrentUser { get; set; }
        public RelationShipStatus FriendFlag {get;set;}
        public Guid UserId { get; set; }
      
    }
}