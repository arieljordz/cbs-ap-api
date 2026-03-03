using CbsAp.Application.Abstractions.Services.Shared;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection;

namespace CbsAp.Infrastracture.Persistence.ContextDependencyChecker
{
    public class DbSetDependencyChecker : IDbSetDependencyChecker
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DbSetDependencyChecker> _logger;

        public DbSetDependencyChecker(ApplicationDbContext context, ILogger<DbSetDependencyChecker> Logger)
        {
            _context = context;
            _logger = Logger;
        }

        public async Task<DependencyCheckerType> HasDependenciesAsync<TEntity>(object id) where TEntity : class
        {
            try
            {
                var entityType = _context.Model.FindEntityType(typeof(TEntity));
                if (entityType == null)
                    return DependencyCheckerType.EntityNotFound;

                // Find all foreign keys where TEntity is the principal (parent)
                var foreignKeys = _context.Model.GetEntityTypes()
                    .SelectMany(e => e.GetForeignKeys())
                    .Where(fk => fk.PrincipalEntityType == entityType)
                    .ToList();

                foreach (var fk in foreignKeys)
                {
                    var dependentClrType = fk.DeclaringEntityType.ClrType;

                    var dependentSet = (IQueryable)_context
                        .GetType()
                        .GetMethod(nameof(DbContext.Set), Type.EmptyTypes)!
                        .MakeGenericMethod(dependentClrType)
                        .Invoke(_context, null)!;

                    var dependentProperty = fk.Properties.First();
                    var parameter = Expression.Parameter(dependentClrType, "e");
                    var property = Expression.Property(parameter, dependentProperty.Name);
                    var constant = Expression.Convert(Expression.Constant(id), property.Type);
                    var equal = Expression.Equal(property, constant);
                    var lambda = Expression.Lambda(equal, parameter);

                    var anyAsync = typeof(EntityFrameworkQueryableExtensions)
                        .GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Where(m => m.Name == "AnyAsync")
                        .First(m =>
                        {
                            var parameters = m.GetParameters();
                            return parameters.Length == 3 &&
                                   parameters[0].ParameterType.IsGenericType &&
                                   parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>) &&
                                   parameters[2].ParameterType == typeof(CancellationToken);
                        })
                        .MakeGenericMethod(dependentClrType);

                    var task = (Task<bool>)anyAsync.Invoke(null, new object[] { dependentSet, lambda, CancellationToken.None })!;
                    if (await task)
                    {
                        return DependencyCheckerType.HasDependencies;
                    }
                }

                // No dependencies found
                return DependencyCheckerType.NoDependencies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dependency check failed for Deleting Object ID : {ID}", id);
                // exception
                return DependencyCheckerType.Error;
            }
        }
    }
}