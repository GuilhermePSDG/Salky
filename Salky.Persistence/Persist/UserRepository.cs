using Microsoft.EntityFrameworkCore;
using Salky.Domain;
using Salky.Persistence.Contexts;

namespace Salky.Persistence.Persist
{
    public class UserRepository
    {
        private readonly SalkyDbContext _context;

        public UserRepository(SalkyDbContext salkyDbContext)
        {
            _context = salkyDbContext;
        }

        private IQueryable<User> baseQuery(bool IncludeAll)
        {
            if (IncludeAll)
                return _context.Users
                    .AsNoTracking()
                    .Include(x => x.UserOwnerList).ThenInclude(x => x.UserContact)
                    .Include(x => x.UserContactList);
            else
                return _context.Users.AsNoTracking();
        }

        public async Task<User?> GetUserByName(string userName, bool includeAll = false)
        {
            return await baseQuery(includeAll).FirstOrDefaultAsync(x => x.UserName.Equals(userName));
        }

        public async Task<User?> GetById(Guid userId, bool includeAll)
        {
            return await baseQuery(includeAll).FirstOrDefaultAsync(x => x.Id.Equals(userId));
        }
        public void Delete(User user)
        {
            _context.ChangeTracker.Clear();
            this._context.Remove(user);
        }
        public Task<int> SaveChangesAsync()
        {
            return this._context.SaveChangesAsync();
        }
    }
}
