using Salky.Domain.Models.FriendModels;
using Salky.Domain.Models.GenericsModels;

namespace Salky.Domain.Contracts
{
    public interface IMessageFriendRepository : IRepositoryBase<FriendMessage>
    {
        public Task<PaginationResult<FriendMessage>> GetAll(Guid friendId, int currentPage, int pageSize);
        public Task<FriendMessage?> GetById(Guid id);
    }
}
