using Salky.Domain.Models.FriendModels;

namespace Salky.Domain.Contracts
{
    public interface IFriendRepository : IRepositoryBase<Friend>
    {
        public Task<List<Friend>> GetAll(Guid userId, bool includeAll = false);
        public Task<List<Friend>> GetAllAproved(Guid userId, bool includeAll = false);
        public Task<List<Friend>> GetAllAprovedOrPending(Guid userId, bool includeAll);
        public Task<List<Friend>> GetAllPending(Guid userId, bool includeAll);

        public bool HasFriend(Guid userId, Guid otherUserId);
        public Task<Friend?> GetById(Guid userId, Guid FriendId, bool IncludeAll);
        public Task<bool> IsOneOfTheFriends(Guid userId, Guid friendId);
        public Task<Friend?> GetByUsersId(Guid userId, Guid otherUserId);

    }
}
