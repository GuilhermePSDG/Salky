using Salky.Domain.Models.FriendModels;

namespace Salky.Domain.Contracts
{
    public interface IFriendRepository : IRepositoryBase<Friend>
    {

        public Task<List<Friend>> GetAllAprovedOrPending(Guid userId, bool includeAll);
        public Task<Friend?> GetById(Guid userId, Guid FriendId, bool IncludeAll);
        public Task<bool> IsOneOfTheFriends(Guid userId, Guid friendId);
        public Task<Friend?> GetByUsersId(Guid userId, Guid otherUserId);

    }
}
