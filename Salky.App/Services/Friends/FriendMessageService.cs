using Salky.App.Interfaces;
using Salky.Domain.Contracts;
using Salky.Domain.Models.FriendModels;
using Salky.Domain.Models.GenericsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.App.Services.Friends
{
    public class FriendMessageService : IFriendMessageService
    {
        private readonly IMessageFriendRepository repo;
        private readonly IFriendRepository friendRepository;

        public FriendMessageService(IMessageFriendRepository msgRepo, IFriendRepository friendRepository)
        {
            throw new NotImplementedException();
            repo = msgRepo;
            this.friendRepository = friendRepository;
        }
        public async Task<FriendMessage?> Add(Guid userId, Guid friendId, string content)
        {
            if (!await friendRepository.IsOneOfTheFriends(userId, friendId)) return null;
            var msg = new FriendMessage()
            {
                Content = content,
                FriendId = friendId,
                SenderId = userId,
                Status = Domain.Models.MessageModels.MessageStatus.notsended,
            };
            repo.Add(msg);
            if (await repo.EnsureSaveChangesAsync() == 0) return null;
            return await repo.GetById(msg.Id);
        }

        public async Task<PaginationResult<FriendMessage>?> GetAll(Guid userId, Guid friendId, int currantePage, int pageSize)
        {
            if (!await friendRepository.IsOneOfTheFriends(userId, friendId)) return null;
            var messages = await repo.GetAll(friendId, currantePage, pageSize);
            return messages;
        }

        public async Task<bool> Remove(Guid userId, Guid messageId)
        {
            var msg = await repo.GetById(messageId);
            if (msg == null) return false;
            if (!msg.HeIsTheSender(userId)) return false;
            repo.Remove(msg);
            return await repo.EnsureSaveChangesAsync() > 0;
        }

        public async Task<bool> Update(Guid userId, Guid messageId, string newContent)
        {
            var msg = await repo.GetById(messageId);
            if (msg == null) return false;
            if (!msg.HeIsTheSender(userId)) return false;
            msg.Content = newContent;
            repo.Update(msg);
            return await repo.EnsureSaveChangesAsync() > 0;
        }

    }
}
