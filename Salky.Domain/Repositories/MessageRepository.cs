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


        public Task<PaginationResult<MessageGroup>> GetByGroupId(Guid groupId, int currentPage, int pageSize)
        {
            var query = db.MessagesGroup
                .Include(f => f.Sender)
                .Where(x => x.GroupId == groupId)
                .OrderByDescending(x => x.CreatedDate);
            var result = new PaginationResult<MessageGroup>(query, currentPage, pageSize);
            result.Results.Reverse();
            return Task.FromResult(result);
        }

        public async Task<MessageGroup?> GetById(Guid id)
        {
            return await GetBaseQuery(true).FirstOrDefaultAsync(q => q.Id.Equals(id));
        }

    }
}
