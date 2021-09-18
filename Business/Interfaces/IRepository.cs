using Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : Entity
    {
        Task CreateAsync(TEntity entity);
        Task CreateArrayAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateArrayAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(TEntity entity);
    }
}
