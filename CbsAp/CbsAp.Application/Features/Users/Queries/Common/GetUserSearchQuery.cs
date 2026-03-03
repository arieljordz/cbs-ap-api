using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared;

namespace CbsAp.Application.Features.Users.Queries.Common
{
    public record GetUserSearchQuery : IQuery<Result<List<UserSearchPaginationDTO>>>;
}