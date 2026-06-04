using CbsAp.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IUnitofWork : IDisposable
    {
        DbContext Context { get; }
        IRepository<T> GetRepository<T>() where T : BaseAuditableEntity;

       


        Task<bool> SaveChanges(string auditUser, string auditModule, CancellationToken cancellationToken);
        Task<IDbContextTransaction> BeginTransactionAsync();

    }
}