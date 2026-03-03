using CbsAp.Domain.Entities.UserManagement;
using System.Linq.Expressions;

namespace CbsAp.Application.Shared.Extensions
{
    public static class SearchCustomPredicates
    {
        public static Expression<Func<UserAccount, bool>> FullNameContains(string searchTerm)
        {
            return p => (p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm));
        }
        // with search with full Name
        public static Expression<Func<T, bool>> SearchFullName<T>(string searchName,
             Expression<Func<T, string>> firstNameSelector,
             Expression<Func<T, string>> lastNameSelector
            )
        {
            return p => (firstNameSelector.Compile()(p) + " " + lastNameSelector.Compile()(p))
                        .Contains(searchName);
        }

        public static Expression<Func<T, bool>> SearchField<T>(string searchCriteria,
             Expression<Func<T, string>> criteria
            )
        {
            return p => (criteria.Compile()(p)).Contains(searchCriteria);
        }
    }
}