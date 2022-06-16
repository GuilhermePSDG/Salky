using Salky.Domain.Models.UserModels;

namespace Salky.Domain.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        public Task<List<User>> FindUsersByName(string userName, bool includeAll = false, int MaxUsers = 10);
        public Task<User?> GetUserByName(string userName, bool includeAll = false);
        public Task<User?> GetById(Guid userId, bool includeAll);
        public bool Exist(Guid userid);
        public bool Exist(string UserName);
    }
}
