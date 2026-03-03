using CbsAp.Domain.Common;
using System.Linq.Expressions;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IRepository<T> where T : BaseAuditableEntity
    {
        Task<T> GetByIdAsync(int id);

        Task<T> GetByIdAsync(long key);

        IQueryable<T> Query();

        Task<IQueryable<T>> GetAllAsync();

        Task<T> AddAsync(T entity);

        Task UpdateAsync(long key, T entity);

        Task UpdateAsync(Func<T, bool> condition, Action<T> updateAction);

        Task DeleteAsync(T entity);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<IQueryable<T>> ApplyPredicateAsync(Expression<Func<T, bool>> predicate);

        Task AddRangeAsync(IEnumerable<T> entity);

        Task UpdateRangeAsync(IEnumerable<T> entity);

        Task RemoveRangeAsync(IEnumerable<T> entity);

        Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}