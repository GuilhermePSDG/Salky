using Salky.Domain;

namespace Salky.Persistence.Contracts
{
    public interface IGroupRepository : IRepositoryBase<Group>
    {
        public Task<List<Group>> GetAllGroupsOfUser(Guid userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns>
        /// <see langword="true if"/> <see cref="User"/> is in <see cref="Group.Members"/> otherwise <see langword="false"/>
        /// 
        /// 
        /// </returns>
        public Task<bool> UserIsInGroup(Guid userId, Guid groupId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns><see cref="Group.Members"/></returns>
        public Task<List<User>> GetUsersOfGroup(Guid groupId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="includeAll"></param>
        /// <returns><code> The <see cref="Group"/> <see langword="if"/> <see cref="User"/> <see langword="is in"/> <see cref="Group"/> otherwise <see langword="null"/>
        /// 
        /// </code></returns>
        public Task<Group?> GetGroupById(Guid userId, Guid groupId, bool includeAll);
  
    }
}
