using Microsoft.EntityFrameworkCore;
using Salky.Domain;
using Salky.Persistence.Contexts;
using Salky.Persistence.Contracts;

namespace Salky.Persistence.Repositories
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(SalkyDbContext dbctx) : base(dbctx)
        {
        }

        private IQueryable<Group> getBaseQuery(bool IncldueAll)
        {
            if (IncldueAll)
            {
                return db.Groups
                    .AsNoTracking()
                    .Include(x => x.Config)
                    .Include(x => x.Messages)
                    .Include(x => x.Members).ThenInclude(x => x.User);
            }
            else
            {
                return db.Groups
                    .AsNoTracking()
                    .Include(x => x.Config);
            }
        }

        public async Task<List<Group>> GetAllGroupsOfUser(Guid userId)
        {
            return await getBaseQuery(false)
                .Where(x => x.Members.Any(q => q.UserId.Equals(userId)))
                .ToListAsync();
        }

        public async Task<bool> UserIsInGroup(Guid userId, Guid groupId)
        {
            return await getBaseQuery(false)
                .Where(x => x.Id == groupId)
                .Select(x => x.Members.Any(q => q.UserId == userId))
                .FirstAsync();
        }

        public async Task<List<User>> GetUsersOfGroup(Guid groupId)
        {
            var r = await db.Groups
                .AsNoTracking()
                .Include(x => x.Members).ThenInclude(x => x.User)
                .Where(x => x.Id.Equals(groupId))
                .SelectMany(f => f.Members.Select(x => x.User))
                .ToListAsync();
            return r;
        }

        public async Task<Group?> GetGroupById(Guid userId, Guid groupId, bool includeAll)
        {
            return await getBaseQuery(includeAll)
                .Where(x => x.Id.Equals(groupId) && x.Members.Any(q => q.UserId == userId))
                .FirstOrDefaultAsync();
        }

        public bool IsMemberOfGroup(Guid userId, Guid groupId)
        {
            return db.Groups.Any(f => f.Id == groupId && f.Members.Any(x => x.UserId == userId));
        }

    }
}
