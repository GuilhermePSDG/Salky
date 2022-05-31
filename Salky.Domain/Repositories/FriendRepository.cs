using Microsoft.EntityFrameworkCore;
using Salky.Domain.Contracts;
using Salky.Domain.Contexts;
using Salky.Domain.Models.FriendModels;

namespace Salky.Domain.Repositories
{
    public class FriendRepository : RepositoryBase<Friend>, IFriendRepository
    {
        public FriendRepository(SalkyDbContext db) : base(db)
        {

        }

        public async Task<List<Friend>> GetAll(Guid userId, bool includeAll = false)
        {
            return await Query(includeAll)
                .Where(x => x.RequestedById == userId || x.RequestedToId == userId)
                .ToListAsync();
        }

        public async Task<List<Friend>> GetAllAprovedOrPending(Guid userId, bool includeAll)
        {
            return await Query(includeAll)
            .Where(x => (x.RequestedById == userId || x.RequestedToId == userId) && (x.FriendRequestFlag == RelationShipStatus.Approved || x.FriendRequestFlag == RelationShipStatus.Pending))
            .ToListAsync();
        }

        public async Task<List<Friend>> GetAllPending(Guid userId, bool includeAll)
        {
            return await Query(includeAll)
                .Where(x => (x.RequestedById == userId || x.RequestedToId == userId) && x.FriendRequestFlag == RelationShipStatus.Pending)
                .ToListAsync();
        }

        public async Task<List<Friend>> GetAllAproved(Guid userId, bool includeAll = false)
        {
            return await Query(includeAll)
                .Where(x => (x.RequestedById == userId || x.RequestedToId == userId) && x.FriendRequestFlag == RelationShipStatus.Approved)
                .ToListAsync();
        }

        public bool HasFriend(Guid userId, Guid otherUserId)
        {
            return Query(false).Any(x =>
           x.RequestedById == userId && x.RequestedToId == otherUserId
           ||
           x.RequestedToId == userId && x.RequestedById == otherUserId
            );
        }


        public async Task<Friend?> GetByUsersId(Guid userId, Guid otherUserId)
        {
            return await Query(false).FirstOrDefaultAsync(x =>
           x.RequestedById == userId && x.RequestedToId == otherUserId
           ||
           x.RequestedToId == userId && x.RequestedById == otherUserId
            );
        }


        public async Task<Friend?> GetById(Guid userId, Guid FriendId, bool IncludeAll)
        {
            return await Query(IncludeAll).SingleOrDefaultAsync(
                x => x.Id == FriendId && (x.RequestedById == userId || x.RequestedToId == userId)
                );
        }


        private IQueryable<Friend> Query(bool includeAll)
        {
            var query = db.Friend.AsNoTracking();
            if (includeAll)
            {
                query = query
                    .Include(x => x.RequestedBy)
                    .Include(x => x.RequestedTo);
            }
            return query;
        }

        public async Task<bool> IsOneOfTheFriends(Guid userId,Guid friendId)
        {
            return await this.db.Friend
                .AnyAsync(x => x.Id == friendId && (x.RequestedById == userId || x.RequestedToId == userId));
        }

        public override void Remove(Friend entity)
        {
            throw new NotImplementedException();
        }

    }
}
