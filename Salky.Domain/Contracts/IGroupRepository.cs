using Salky.Domain.Models.GroupModels;
using Salky.Domain.Models.UserModels;

namespace Salky.Domain.Contracts
{
    public interface IGroupRepository : IRepositoryBase<Group>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<Group>> GetAllGroupsOfUser(Guid userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="includeAll"></param>
        /// <returns><code> The <see cref="Group"/> <see langword="if"/> exist and if the <see cref="User"/> <see langword="is in"/> <see cref="Group"/> otherwise <see langword="null"/>
        /// 
        /// </code></returns>
        //public Task<Group?> GetGroupById(Guid userId, Guid groupId, bool includeAll);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="includeAll"></param>
        /// </returns>
        public Task<Group?> GetGroupByIdWithRolesAndConfig(Guid groupId);
        Task<Group?> GetGroupByIdWithMembersWithTracking(Guid groupId);
        public Task<Group?> GetGroupById(Guid groupId);

    }
}
