using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CbsAp.Application.Shared.Extensions
{
    public static class IQueryableExtension
    {
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> source,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken) where T : class
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = await source.CountAsync(cancellationToken);

            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            List<T> items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync(cancellationToken);

            return PaginatedList<T>.Create(items, count, pageNumber, pageSize);
        }

        public static Task<PaginatedList<T>> ToPaginatedListAsync<T>(
             this IEnumerable<T> source,
             int pageNumber,
             int pageSize,
             CancellationToken cancellationToken) where T : class
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            int count = source.Count();

            var items = source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Task.FromResult(PaginatedList<T>.Create(items, count, pageNumber, pageSize));
        }

        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string? sortField, int? sortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
                return source;

            var parameter = Expression.Parameter(typeof(T), "p");
            Expression property;

            if (sortField == "fullName")
            {
                var firstName = Expression.Property(parameter, "FirstName");
                var lastName = Expression.Property(parameter, "LastName");
                var concatMethod = typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) });

                var space = Expression.Constant(" ");

                var firstNameWithSpace = Expression.Call(concatMethod!, firstName, space);

                property = Expression.Call(concatMethod!, firstNameWithSpace, lastName);
            }
            else
            {
                // Single property sorting
                property = Expression.Property(parameter, sortField);
            }
            var keySelector = Expression.Lambda(property, parameter);

            var methodName = sortOrder == -1 ? "OrderByDescending" : "OrderBy";
            var method = typeof(Queryable).GetMethods()
                .Single(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type);

            var result = method.Invoke(null, new object[] { source, keySelector });
            return (IQueryable<T>)result!;
        }

        public static IEnumerable<T> OrderByDynamic<T>(this IEnumerable<T> source, string? sortField, int? sortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
                return source;

            var param = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(param, sortField);
            var converted = Expression.Convert(property, typeof(object));
            var keySelector = Expression.Lambda<Func<T, object>>(converted, param).Compile();

            return sortOrder == -1
                ? source.OrderByDescending(keySelector)
                : source.OrderBy(keySelector);
        }
    }
}