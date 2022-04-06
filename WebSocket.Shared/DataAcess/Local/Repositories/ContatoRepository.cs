
using Microsoft.EntityFrameworkCore;
using WebSocket.Shared.DataAcess.Models;

namespace WebSocket.Shared.DataAcess.Local.Repositories
{
    public class ContatoRepository : RepositoryBase<Contato>
    {
        public ContatoRepository(SalkySqlLiteDbContext dbcontext) : base(dbcontext)
        {
        }
        public override IQueryable<Contato> GetQuery()
        {
            if (ToTrack)
                return this.dbcontext.Contatos
                    .Include(x => x.Messages);
            else
                return this.dbcontext.Contatos.AsNoTracking()
                    .Include(x => x.Messages);
        }
    }
}
