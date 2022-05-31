using Microsoft.EntityFrameworkCore;
using Salky.Domain.Contexts;
using Salky.Domain.Contracts;
using Salky.Domain.Models.GroupModels;
using Salky.Domain.Models.UserModels;

namespace Salky.Domain.Repositories
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
                    .Include(x => x.Members).ThenInclude(x => x.User)
                    .Include(x => x.GroupRoles);
            }
            else
            {
                return db.Groups
                    .AsNoTracking()
                    .Include(x => x.Config)
                    .Include(x => x.GroupRoles);
            }
        }

        public async Task<List<Group>> GetAllGroupsOfUser(Guid userId)
        {
            return await getBaseQuery(false)
                .Where(x => x.Members.Any(q => q.UserId.Equals(userId)))
                .ToListAsync();
        }
        public async Task<Group?> GetGroupByIdWithRolesAndConfig(Guid groupId)
        {
            return await getBaseQuery(false).FirstOrDefaultAsync(x => x.Id == groupId);
        }

        public async Task<Group?> GetGroupById(Guid groupId)
        {
            return await this.db.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId);
        }

        public async Task<Group?> GetGroupByIdWithMembersWithTracking(Guid groupId)
        {
            var res = await db.Groups
                .Include(x => x.Config)
                .Include(x => x.GroupRoles)
                .Include(m => m.Members)
                .ThenInclude(x => x.Role).ThenInclude(x => x.GroupPermissions)
                .AsSplitQuery()
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == groupId);
            return res;
        }

        public override void Remove(Group entity)
        {
            var g = this.db.Groups.AsNoTracking().First(x => x.Id == entity.Id);
            this.db.Groups.Remove(g);
        }

    }
}
