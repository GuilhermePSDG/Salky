using Salky.Domain.Models.GroupModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Domain.Contracts
{
    public interface IGroupMemberRepository : IRepositoryBase<GroupMember>
    {

        /// <summary>
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="GroupId"></param>
        /// <returns><see cref="List{GroupMember}"/></returns>
        public Task<List<GroupMember>> GetMembers(Guid GroupId);
        Task<GroupMember?> GetMemberByUserId(Guid UserId, Guid GroupId);
        public Task<bool> HasMember(Guid MemberId, Guid groupId);
        public Task<bool> HasMember(Guid MemberId);
        public Task<bool> HasMemberByUserId(Guid UserId, Guid GroupId);
        public Task<GroupMember?> GetMemberByUserIdWithRole(Guid UserId, Guid GroupId);
        Task<GroupMember?> GetMemberById(Guid MemberId);
        Task<GroupMember?> GetMemberById(Guid MemberId,Guid GroupId);
        Task<List<GroupMember>> GetAllMembersOfUser(Guid userId);
    }
}
