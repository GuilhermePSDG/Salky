using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Shared.DataAcess.Models;

namespace WebSocket.Shared.DataAcess.Local.Repositories
{
    public class UsuarioRepository : IRepository<Usuario>
    {
        private SalkySqlLiteDbContext dbcontext;

        public UsuarioRepository(SalkySqlLiteDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public void Add(Usuario entity)
        {
            this.dbcontext.Add(entity);
        }

        public Usuario? Find(Func<Usuario, bool> expression)
        {
            return this.dbcontext.Usuarios.Where(expression).FirstOrDefault();
        }

        public List<Usuario>? FindAll(Func<Usuario, bool> expression)
        {
            return this.dbcontext.Usuarios.Where(expression).ToList();
        }

        public List<Usuario>? GetAll()
        {
            return this.dbcontext.Usuarios.ToList();
        }

        public Usuario? GetById(int id)
        {
            return this.dbcontext.Usuarios.FirstOrDefault(x => x.Id == id); 
        }

        public void Remove(Usuario entity)
        {
            this.dbcontext.Usuarios.Remove(entity);
        }

        public void Remove(int id)
        {
            var entity = GetById(id);
            if(entity == null) throw new ArgumentNullException(nameof(entity)); 
            this.dbcontext.Usuarios.Remove(entity);
        }


        public void Update(Usuario entity)
        {
            this.dbcontext.Usuarios.Update(entity);
        }


        public void SaveChanges()
        {
            this.dbcontext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.dbcontext.SaveChangesAsync();    
        }
    }
}
