
using Microsoft.EntityFrameworkCore;
using WebSocket.Shared.DataAcess.Models;

namespace WebSocket.Shared.DataAcess.Local.Repositories
{
    public class MessageRepository : RepositoryBase<Message>
    {
        public MessageRepository(SalkySqlLiteDbContext dbcontext) : base(dbcontext)
        {
        }
        public override IQueryable<Message> GetQuery()
        {
            if (ToTrack)
                return this.dbcontext.Mensagens;
            else
                return this.dbcontext.Mensagens.AsNoTracking();
        }
    }
}
