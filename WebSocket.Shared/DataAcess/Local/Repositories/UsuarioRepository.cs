
using Microsoft.EntityFrameworkCore;
using WebSocket.Shared.DataAcess.Models;

namespace WebSocket.Shared.DataAcess.Local.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>
    {

        public UsuarioRepository(SalkySqlLiteDbContext dbcontext) : base(dbcontext)
        {
        }

        public override IQueryable<Usuario> GetQuery()
        {
            if(ToTrack)
                return this.dbcontext.Usuarios
                    .Include(x => x.Contatos);
            else
                return this.dbcontext.Usuarios.AsNoTracking()
                .Include(x => x.Contatos).ThenInclude(x => x.Messages);
        }
    }
}
