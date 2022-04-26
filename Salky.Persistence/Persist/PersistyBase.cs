using Microsoft.EntityFrameworkCore;
using Salky.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Persistence.Persist
{
    public class PersistyBase<T> where T : notnull
    {
        public SalkyDbContext db;

        public PersistyBase(SalkyDbContext dbctx)
        {
            db = dbctx;
        }

        public virtual void Add(T entity)
        {
            db.Add(entity);
        }
        public virtual void Remove(T entity)
        {
            db.Remove(entity);
        }
        public virtual void Update(T entity)
        {
            db.Update(entity);
        }
        public Task<int> SaveChangesAsync()
        {
            var saveCount =  db.SaveChangesAsync();
            db.ChangeTracker.Clear();
            return saveCount;
        }
    }
}
