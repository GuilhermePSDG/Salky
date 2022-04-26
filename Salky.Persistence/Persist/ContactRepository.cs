using Microsoft.EntityFrameworkCore;
using Salky.Domain;
using Salky.Persistence.Contexts;

namespace Salky.Persistence.Persist
{
    public class ContactRepository : PersistyBase<Contato>
    {
        private IQueryable<Contato> baseQuery(bool IncludeAll)
        {
            if (IncludeAll)
                return db.Contacts.AsNoTracking()
                    .Include(x => x.UserOwner)
                    .Include(x => x.UserContact);
            else
                return db.Contacts.AsNoTracking();
        }
        public ContactRepository(SalkyDbContext dbcontext) : base(dbcontext)
        {

        }

        public void AddContact(Guid currentUserId, Guid contactId)
        {
            db.Add(new Contato()
            {
                UserOwnerId = currentUserId,
                UserContactId = contactId,
            });
        }

        public async Task<List<Contato>> GetAllByUserId(Guid userId, bool includeAll)
        {
            return await baseQuery(includeAll)
                .Where(x => x.UserOwnerId.Equals(userId)).ToListAsync();
        }

        public async Task<Contato?> GetById(Guid ownerId,Guid ContatoId, bool includeAll)
        {
            return await baseQuery(includeAll).SingleOrDefaultAsync(f => f.Id.Equals(ContatoId) && f.UserOwnerId.Equals(ownerId));
        }


        public async Task<Contato?> GetContactByUsersIds(Guid currentUserId, Guid UserContactId, bool includeAll)
        {
            return await baseQuery(includeAll)
                .SingleOrDefaultAsync(x => x.UserOwnerId.Equals(currentUserId) && x.UserContactId.Equals(UserContactId));
        }

        public async Task<bool> HasContact(Guid currentUserId, Guid contactId)
        {
            return await db.Contacts.AnyAsync(x => x.UserOwnerId.Equals(currentUserId) && x.UserContactId.Equals(contactId));
        }

        public async Task<bool> AreFriends(Guid usr1Id, Guid usr2Id)
        {
            return (await HasContact(usr1Id, usr2Id)) && (await HasContact(usr2Id, usr1Id));
        }



    }
}
