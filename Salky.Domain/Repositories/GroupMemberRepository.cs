using Microsoft.EntityFrameworkCore;
using Salky.Domain.Contexts;
using Salky.Domain.Contracts;
using Salky.Domain.Models.GroupModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Domain.Repositories
{
    public class GroupMemberRepository : RepositoryBase<GroupMember>, IGroupMemberRepository
    {
        public GroupMemberRepository(SalkyDbContext db) : base(db)
        {

        }
        private IQueryable<GroupMember> getBaseQuery(bool includeAll)
        {
            var query = this.db.GroupsUsers
                .AsNoTracking();
            if (includeAll)
            {
                query = query.Include(x => x.Group);
            }
           return query.Include(x => x.User).Include(q => q.Role);
        }
        public async Task<bool> HasMemberByUserId(Guid UserId, Guid groupId)
        {
            return await getBaseQuery(false).AnyAsync(x => x.UserId == UserId && x.GroupId == groupId);
        }
        public async Task<bool> HasMember(Guid memberId, Guid groupId)
        {
            return await getBaseQuery(false).AnyAsync(x => x.Id == memberId && x.GroupId == groupId);
        }
        public async Task<bool> HasMember(Guid memberId)
        {
            return await getBaseQuery(false).AnyAsync(x => x.Id == memberId);
        }
        public async Task<GroupMember?> GetMemberByUserId(Guid UserId, Guid GroupId)
        {
            return await getBaseQuery(false).FirstOrDefaultAsync(x => x.GroupId == GroupId && x.UserId == UserId);
        }
        public async Task<GroupMember?> GetMemberByUserIdWithRole(Guid UserId, Guid GroupId)
        {
            return await getBaseQuery(false)
                .Include(x => x.Role).ThenInclude(x => x.ChatPermissions)
                .Include(x => x.Role).ThenInclude(x => x.CallPermisions)
                .Include(x => x.Role).ThenInclude(x => x.GroupPermissions)
                .FirstOrDefaultAsync(x => x.GroupId == GroupId && x.UserId == UserId);
        }

        public async Task<List<GroupMember>> GetMembers(Guid GroupId)
        {
            return await getBaseQuery(false).Where(x => x.GroupId == GroupId).ToListAsync(); 
        }

        public async Task<GroupMember?> GetMemberById(Guid MemberId, Guid GroupId)
        {
            return await getBaseQuery(false).FirstOrDefaultAsync(x => x.Id == MemberId && x.GroupId == GroupId);
        }
        public async Task<GroupMember?> GetMemberById(Guid MemberId)
        {
            return await getBaseQuery(false).FirstOrDefaultAsync(x => x.Id == MemberId );
        }

        public async Task<List<GroupMember>> GetAllMembersOfUser(Guid userId)
        {
            return await getBaseQuery(false).Where(x => x.UserId == userId).ToListAsync();
        }
    }

}
