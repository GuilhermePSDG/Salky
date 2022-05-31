using Salky.Domain;
using Salky.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Salky.Persistence.Contracts;

namespace Salky.Persistence.Repositories
{
    public class FriendRepository :  RepositoryBase<Friend>, IFriendRepository
    {
        public FriendRepository(SalkyDbContext db) :base(db)
        {

        }

        public async Task<List<Friend>> GetAll(Guid userId,bool includeAll = false)
        {
            return await Query(includeAll)
                .Where(x => x.RequestedById == userId || x.RequestedToId == userId )
                .ToListAsync();
        }

        public async Task<List<Friend>> GetAllAccepted(Guid userId,bool includeAll = false)
        {
            return await Query(includeAll)
                .Where(x => (x.RequestedById == userId || x.RequestedToId == userId) && x.FriendRequestFlag == FriendRequestFlag.Approved)
                .ToListAsync();
        }



        public  bool HasFriend(Guid userId,Guid otherUserId)
        {
            return  Query(false).Any(x =>
            x.RequestedById == userId && x.RequestedToId == otherUserId
            ||
            x.RequestedToId == userId && x.RequestedById == otherUserId
            );
        }

        public async Task<Friend?> GetById(Guid userId,Guid FriendId,bool IncludeAll)
        {
            return await Query(IncludeAll).SingleOrDefaultAsync(
                x => x.Id.Equals(FriendId) && (x.RequestedById == userId || x.RequestedToId == userId)
                );
        }

 
        private IQueryable<Friend> Query(bool includeAll)
        {
            var query = this.db.Friend.AsNoTracking();
            if (includeAll)
            {
                query = query
                    .Include(x => x.RequestedBy)
                    .Include(x => x.RequestedTo);
            }
            return query;
        }

    }
}
