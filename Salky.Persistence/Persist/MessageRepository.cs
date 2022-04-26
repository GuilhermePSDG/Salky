using Microsoft.EntityFrameworkCore;
using Salky.Domain.Salky.Domain;
using Salky.Persistence.Contexts;


namespace Salky.Persistence.Persist
{
    public class MessageRepository : PersistyBase<Message>
    {
        public MessageRepository(SalkyDbContext dbctx) : base(dbctx) { }
       
        private IQueryable<Message> GetBaseQuery(bool includeAll)
        => includeAll ? 
            this.db.Messages
            .AsNoTracking()
            .Include(f=>f.Contato) : 
            this.db.Messages.AsNoTracking();
        

        public async Task<List<Message>> GetByContactId(Guid currentUserId,Guid contactId)
        {
            return await GetBaseQuery(true)
                .Where
                (x => 
                x.ContatoId.Equals(contactId) && 
                x.Contato.UserOwnerId.Equals(currentUserId)
                        )
                .ToListAsync();
        }

        public async Task<Message?> GetByMessageId(Guid currentUserId,Guid messageId)
        {
            return await GetBaseQuery(true).SingleOrDefaultAsync(f => f.Id.Equals(messageId) && f.Contato.UserOwnerId.Equals(currentUserId));
        }

    }
}
