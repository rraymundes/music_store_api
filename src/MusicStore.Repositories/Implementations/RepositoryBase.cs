using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;
using MusicStore.Repositories.Abstractions;
using System.Linq.Expressions;

namespace MusicStore.Repositories.Implementations
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        protected readonly DbContext _context;

        protected RepositoryBase(DbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>()
                .AddAsync(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await GetAsync(id);

            if ((item is not null))
            {
                item.Status = false;
                await UpdateAsync();
            }
        }

        public virtual async Task<ICollection<TEntity>> GetAsync()
        {
            return await _context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>()
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<TEntity>> GetAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy)
        {
            return await _context.Set<TEntity>()
                .Where(predicate)
                .OrderBy(orderBy)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TEntity?> GetAsync(int id)
        {
            return await _context.Set<TEntity>()
                .FindAsync(id);
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
