using Salky.Domain.Salky.Domain;
using Salky.Persistence.Models;

namespace Salky.Persistence.Contracts
{
    public interface IMessageRepository : IRepositoryBase<Message>
    {
        public Task<PaginationResult<Message>> GetByGroupId(Guid currentUserId, Guid groupId, int currentPage, int pageSize);
        public Task<Message?> GetById(Guid id);
    }
}
