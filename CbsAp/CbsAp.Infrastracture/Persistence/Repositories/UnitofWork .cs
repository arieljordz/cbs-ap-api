using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Domain.Common;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class UnitofWork : IUnitofWork
    {
        private bool disposed;
        private Hashtable? repositories;
        private readonly ApplicationDbContext _dbContext;

        

        public UnitofWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public DbContext Context => _dbContext;
        public IRepository<T> GetRepository<T>() where T : BaseAuditableEntity
        {
            if (repositories == null)
                repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);
                repositories.Add(type, repositoryInstance);
            }
            return (IRepository<T>)repositories[type]!;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task<bool> SaveChanges(string auditUser, string auditModule,CancellationToken cancellationToken)
        {
            _dbContext.AuditUser = auditUser;
            _dbContext.AuditModule = auditModule;
            return await _dbContext.SaveChangesAsync(cancellationToken) >= 0;
        }

        


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}