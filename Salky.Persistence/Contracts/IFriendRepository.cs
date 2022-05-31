using Salky.Domain;

namespace Salky.Persistence.Contracts
{
    public interface IFriendRepository : IRepositoryBase<Friend>
    {
        public Task<List<Friend>> GetAll(Guid userId, bool includeAll = false);
        public Task<List<Friend>> GetAllAccepted(Guid userId, bool includeAll = false);
        public bool HasFriend(Guid userId, Guid otherUserId);
        public Task<Friend?> GetById(Guid userId,Guid FriendId, bool IncludeAll);
    }
}
