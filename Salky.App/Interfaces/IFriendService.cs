using Salky.App.Dtos;
using Salky.App.Dtos.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.App.Interfaces
{
    public interface IFriendService
    {
        public Task<List<UserFriendDto>> GetAll(Guid userId);
        public Task<List<UserFriendDto>> GetAllAccepted(Guid userId);
        public Task<UserFriendDto?> SendFriendRequest(Guid userId, Guid otherUserId);
        public Task<UserFriendDto?> GetById(Guid LoggedUserId, Guid FriendId);
        
        public Task<MessageDto> SendMessageToFriend(Guid userId, Guid FriendId);



        
        public bool AcceptFriend(Guid userId, Guid friendId);
        public bool RemoveFriend(Guid userId, Guid otherUser);
        public bool BlockFriend(Guid userId, Guid otherUser);
        public bool RejectFriend(Guid userId, Guid otherUser);
        public bool HasFriend(Guid userId, Guid otherUserId);

    }
}
