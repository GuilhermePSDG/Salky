using Microsoft.EntityFrameworkCore;
using Salky.Domain.Contexts;
using Salky.Domain.Contracts;
using Salky.Domain.Models.UserModels;

namespace Salky.Domain.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(SalkyDbContext salkyDbContext) : base(salkyDbContext)
        {
        }

        private IQueryable<User> baseQuery(bool IncludeAll)
        {
            if (IncludeAll)
                return db.Users
                    .Include(x => x.SentFriendRequests)
                    .Include(x => x.ReceievedFriendRequests)
                    .AsSplitQuery()
                    .AsNoTracking();
            else
                return db.Users.AsNoTracking();
        }

        public async Task<List<User>> FindUsersByName(string userName, bool includeAll = false, int MaxResults = 10)
        {
            userName = userName.Trim().ToUpper();
            return await baseQuery(includeAll).Where(x => x.NormalizedUserName.Contains(userName)).Take(MaxResults).ToListAsync();
        }

        public async Task<User?> GetUserByName(string userName, bool includeAll = false)
        {
            return await baseQuery(includeAll).FirstOrDefaultAsync(x => x.NormalizedUserName == userName.ToUpper());
        }

        public async Task<User?> GetById(Guid userId, bool includeAll)
        {
            return await baseQuery(includeAll).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public bool Exist(Guid userid)
        {
            return db.Users.AsNoTracking().Any(x => x.Id == userid);
        }

    }
}
