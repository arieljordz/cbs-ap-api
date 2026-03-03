using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Queries.Common
{
    public record SearchUserByUserAccountIDQuery(long userAccountID) :
        IQuery<ResponseResult<IEnumerable<UserSearchPaginationDTO>>>;
}