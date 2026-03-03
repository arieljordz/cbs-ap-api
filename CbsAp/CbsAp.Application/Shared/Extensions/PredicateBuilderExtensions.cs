using LinqKit;
using System.Linq.Expressions;

namespace CbsAp.Application.Shared.Extensions
{
    public static class PredicateBuilderExtensions
    {
        public static ExpressionStarter<T> AndIf<T>(
        this ExpressionStarter<T> predicate,
        bool condition,
        Expression<Func<T, bool>> expression)
        {
            return condition ? predicate.And(expression) : predicate;
        }
    }
}
