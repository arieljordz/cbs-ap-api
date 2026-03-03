using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Common;
using CbsAp.Infrastracture.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseAuditableEntity
    {
        private readonly ApplicationDbContext _dbcontext;

        public Repository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<T> AddAsync(T entity)
        {
            entity.SetAuditFieldsOnCreate(entity.CreatedBy!);
            await _dbcontext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entity)
        {
            await _dbcontext.Set<T>().AddRangeAsync(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbcontext.Set<T>().AnyAsync(predicate);
        }

        public async Task<IQueryable<T>> ApplyPredicateAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _dbcontext.Set<T>()
                .AsNoTracking()
                .Where(predicate)
                .AsQueryable();

            return await Task.FromResult(query);
        }

        public Task DeleteAsync(T entity)
        {
            _dbcontext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entity)
        {
            _dbcontext.Set<T>().RemoveRange(entity);
        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
            var entities = _dbcontext.Set<T>()
                .AsNoTracking()
                .AsQueryable();
            return await Task.FromResult(entities);
        }

        public IQueryable<T> Query()
        {
            return _dbcontext.Set<T>().AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var result = await _dbcontext.Set<T>().FindAsync(id);
            return await Task.FromResult(result!);
        }

        public async Task<T> GetByIdAsync(long key)
        {
            var result = await _dbcontext.Set<T>().FindAsync(key);
            return await Task.FromResult(result!);
        }

        public Task UpdateAsync(Func<T, bool> condition, Action<T> updateAction)
        {
            var entityToUpdate = _dbcontext.Set<T>().Where(condition).SingleOrDefault();
            if (entityToUpdate != null)
            {
                updateAction.Invoke(entityToUpdate);
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync(long key, T entity)
        {
            T? exist = _dbcontext.Set<T>().Find(key);
            _dbcontext.Entry(exist!).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entity)
        {
            _dbcontext.Set<T>().UpdateRange(entity);
        }

        public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbcontext.Set<T>()
               .AsNoTracking()
               .SingleOrDefaultAsync(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbcontext.Set<T>()
               .AsNoTracking()
               .FirstOrDefaultAsync(predicate);
        }
    }
}