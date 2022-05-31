using Microsoft.EntityFrameworkCore;
using Salky.Domain.Salky.Domain;
using Salky.Persistence.Contexts;
using Salky.Persistence.Contracts;
using Salky.Persistence.Models;

namespace Salky.Persistence.Repositories
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(SalkyDbContext dbctx) : base(dbctx) { }

        private IQueryable<Message> GetBaseQuery(bool includeAll)
        => includeAll ?
            db.Messages
            .AsNoTracking()
            .Include(f => f.Sender)
            :
            db.Messages.AsNoTracking();


        public async Task<PaginationResult<Message>> GetByGroupId(Guid currentUserId, Guid groupId, int currentPage, int pageSize)
        {
            return await PaginationResult<Message>
                .CreateNewAsync(
                query: db.Groups
                .AsNoTracking()
                .Include(x => x.Messages).ThenInclude(f => f.Sender)
                .Where(x => x.Id == groupId && x.Members.Any(f => f.UserId.Equals(currentUserId)))
                .SelectMany(x => x.Messages),
                currentPage: currentPage,
                PageSize: pageSize);

        }

        public async Task<Message?> GetById(Guid id)
        {
            return await GetBaseQuery(true).SingleOrDefaultAsync(q => q.Id.Equals(id));
        }

    }
}
