using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Repositories
{
    public interface IRepository<TEntity, in TKey>
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate);
        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<TEntity?> GetByIdCompositeKeyAsync(TKey id1, TKey id2);
        Task<TEntity> AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        TEntity Update(TEntity entity);
        TEntity Remove(TEntity entity);
        public TEntity RemoveCompositeKey(TKey id1, TKey id2);
        Task<int> Commit();
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetTopNItems<TKeyProperty>(Expression<Func<TEntity, TKeyProperty>> keySelector, int n);
    }
}
