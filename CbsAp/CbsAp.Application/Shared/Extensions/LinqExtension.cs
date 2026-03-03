using System.Linq.Expressions;

namespace CbsAp.Application.Shared.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition,
                Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }
    }
}