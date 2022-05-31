using Salky.Domain.Models.GenericsModels;
using Salky.Domain.Models.GroupModels;
using Salky.Domain.Salky.Domain;

namespace Salky.Domain.Contracts
{
    public interface IMessageGroupRepository : IRepositoryBase<MessageGroup>
    {
        public Task<PaginationResult<MessageGroup>> GetByGroupId( Guid groupId, int currentPage, int pageSize);
        public Task<MessageGroup?> GetById(Guid id);
    }

}

