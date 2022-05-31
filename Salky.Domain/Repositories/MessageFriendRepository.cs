using Microsoft.EntityFrameworkCore;
using Salky.Domain.Contexts;
using Salky.Domain.Contracts;
using Salky.Domain.Models.FriendModels;
using Salky.Domain.Models.GenericsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Domain.Repositories
{
    public class MessageFriendRepository : RepositoryBase<FriendMessage>,IMessageFriendRepository
    {
   
        public MessageFriendRepository(SalkyDbContext db):base(db)
        {

        }
        public async Task<PaginationResult<FriendMessage>> GetAll(Guid friendId, int currentPage, int pageSize)
        {

            return await PaginationResult<FriendMessage>
               .CreateNewAsync(
                GetBaseQuery(true)
                .Where(x => x.FriendId == friendId),
                currentPage,
                pageSize);
        }

        public Task<FriendMessage?> GetById(Guid id)
        {
            return GetBaseQuery(true).SingleOrDefaultAsync(x => x.Id == id);
        }
        private IQueryable<FriendMessage> GetBaseQuery(bool includeAll)
            => includeAll ?
                db.MessagesFriend
                .AsNoTracking()
                .Include(f => f.Sender)
                :
                db.MessagesFriend.AsNoTracking();
    }
}
