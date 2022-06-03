using Microsoft.EntityFrameworkCore;
using Salky.Domain.Contexts;
using Salky.Domain.Contracts;
using Salky.Domain.Models.GenericsModels;
using Salky.Domain.Models.GroupModels;
using Salky.Domain.Salky.Domain;

namespace Salky.Domain.Repositories
{
    public class MessageRepository : RepositoryBase<MessageGroup>, IMessageGroupRepository
    {
        public MessageRepository(SalkyDbContext dbctx
            ) : base(dbctx) { }

        private IQueryable<MessageGroup> GetBaseQuery(bool includeAll)
        => includeAll ?
            db.MessagesGroup
            .AsNoTracking()
            .Include(f => f.Sender)
            :
            db.MessagesGroup.AsNoTracking();


        public async Task<PaginationResult<MessageGroup>> GetByGroupId(Guid groupId, int currentPage, int pageSize)
        {
            return await PaginationResult<MessageGroup>
                .CreateNewAsync(
                query: db.MessagesGroup
                .Include(f => f.Sender)
                .Where(x => x.Id == groupId),
                currentPage: currentPage,
                PageSize: pageSize);

        }

        public async Task<MessageGroup?> GetById(Guid id)
        {
            return await GetBaseQuery(true).FirstOrDefaultAsync(q => q.Id.Equals(id));
        }

    }
}
