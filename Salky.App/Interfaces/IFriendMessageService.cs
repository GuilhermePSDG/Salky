using Salky.Domain.Models.FriendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.App.Interfaces
{
    public interface IFriendMessageService
    {
        public Task<FriendMessage?> Add(Guid userId, Guid friendId,string content);
        public Task<bool> Remove(Guid userId, Guid messageId);
        public Task<bool> Update(Guid userId, Guid messageId, string newContent);



    }
}
