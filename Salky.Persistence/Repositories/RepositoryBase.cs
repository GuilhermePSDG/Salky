using Microsoft.EntityFrameworkCore;
using Salky.Persistence.Contexts;
using Salky.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Persistence.Repositories
{

    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public SalkyDbContext db;

        public RepositoryBase(SalkyDbContext dbctx)
        {
            db = dbctx;
        }

        public virtual void Add(T entity)
        {
            db.Add(entity);
        }
        public virtual void Remove(T entity)
        {
            db.ChangeTracker.Clear();
            db.Remove(entity);
        }
        public virtual void Update(T entity)
        {
            db.Update(entity);
        }
        public async Task<int> SaveChangesAsync()
        {
            var saveCount = await db.SaveChangesAsync();
            db.ChangeTracker.Clear();
            return saveCount;
        }
    }
}
