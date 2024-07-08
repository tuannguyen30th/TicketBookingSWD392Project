using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Repositories
{
    public class GenericRepository<TEntity, TKey> : IRepository<TEntity, TKey>
 where TEntity : class
    {
        private readonly TicketBookingDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(TicketBookingDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
            => _dbSet;

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate)
            => _dbSet.Where(predicate);

        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate) => _dbSet.FirstOrDefault(predicate);

        public async Task<TEntity?> GetByIdAsync(TKey id)
            => await _dbSet.FindAsync(id);
        public async Task<TEntity?> GetByIdCompositeKeyAsync(TKey id1, TKey id2)
            => await _dbSet.FindAsync(id1, id2);
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entityEntry = await _dbContext.Set<TEntity>().AddAsync(entity);
            return entityEntry.Entity;
        }

        public TEntity Update(TEntity entity)
        {
            var entityEntry = _dbContext.Set<TEntity>().Update(entity);
            return entityEntry.Entity;
        }

        public TEntity Remove(TEntity entity)
        {
            var entityEntry = _dbContext.Set<TEntity>().Remove(entity!);
            return entityEntry.Entity;
        }
        public TEntity RemoveCompositeKey(TKey id1, TKey id2)
        {
            var entity = GetByIdCompositeKeyAsync(id1, id2).Result;
            var entityEntry = _dbContext.Set<TEntity>().Remove(entity!);
            return entityEntry.Entity;
        }

        public Task<int> Commit() => _dbContext.SaveChangesAsync();

        public async Task<int> CountAsync()
        {
            var count = await _dbSet.CountAsync();
            return count;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var count = await _dbSet.CountAsync(predicate);
            return count;
        }

        public async Task<IEnumerable<TEntity>> GetTopNItems<TKeyProperty>(Expression<Func<TEntity, TKeyProperty>> keySelector, int n)
        {
            var items = await _dbSet.OrderBy(keySelector).Take(n).ToListAsync();
            return items;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }
    }
}
