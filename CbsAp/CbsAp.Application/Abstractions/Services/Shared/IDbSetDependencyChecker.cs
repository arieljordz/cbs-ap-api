using CbsAp.Domain.Enums;

namespace CbsAp.Application.Abstractions.Services.Shared
{
    public interface IDbSetDependencyChecker
    {
        Task<DependencyCheckerType> HasDependenciesAsync<TEntity>(object id) where TEntity : class;
    }
}