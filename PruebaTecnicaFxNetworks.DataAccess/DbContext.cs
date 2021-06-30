using Microsoft.EntityFrameworkCore;
using PruebaTecnicaFxNetworks.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaFxNetworks.DataAccess
{
    public class DbContext<T> : IDbContext<T> where T :class, IEntity
    {
        DbSet<T> _items;
        ApiDbContext _ctx;

        //the dbcontext is created with all the generic types for greater future compatibility with any business logic
        public DbContext(ApiDbContext apiDbContext) 
        {
            _ctx = apiDbContext;
            _items = _ctx.Set<T>();
        }
        public void Delete(int id)
        {
            var tmp = _items.Find(id);
            if (tmp != null)
            {
                _items.Remove(tmp);
                _ctx.SaveChanges();
            }

        }

        public void DeleteAsync(int id)
        {
            var tmp = _items.Find(id);
            if (tmp != null)
            {
                _items.Remove(tmp);
                _ctx.SaveChangesAsync();
            }
        }

        public IList<T> GetAll()
        {
            return _items.ToList();
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _items.ToListAsync();
        }

        public T GetById(int id)
        {
            return _items.Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _items.FindAsync(id);
        }

        public T Save(T entity)
        {
            if (entity.id.Equals(0))
            {
                _items.Add(entity);
            }
            else
            {
                _items.Update(entity);
            }

            _ctx.SaveChanges();
            return entity;
        }

        public async Task<T> SaveAsync(T entity)
        {
            if (entity.id.Equals(0))
            {
                await _items.AddAsync(entity);
            }
            else
            {
                _items.Update(entity);
            }

            await _ctx.SaveChangesAsync();
            return entity;
        }
    }
}
