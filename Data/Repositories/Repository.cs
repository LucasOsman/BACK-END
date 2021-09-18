using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly ContextDb Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(ContextDb db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await SaveChangesAsync();
        }

        public virtual async Task CreateArrayAsync(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
            await SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChangesAsync();
        }

        public virtual async Task UpdateArrayAsync(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            await SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}
