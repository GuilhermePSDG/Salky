using Salky.Domain;

namespace Salky.Persistence.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        public Task<List<User>> FindUsersByName(string userName, bool includeAll = false);
        public Task<User?> GetUserByName(string userName, bool includeAll = false);
        public Task<User?> GetById(Guid userId, bool includeAll);
        public bool Exist(Guid userid);
    }
}
