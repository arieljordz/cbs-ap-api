using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.AccountDimension;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.AccountDimension.Queries.SearchActions
{
    public class AccountDimensionSearchQueryHandler :
        IQueryHandler<SearchAccountDimensionParamQuery, ResponseResult<PaginatedList<AccountDimensionSearchDTO>>>
    {
        private readonly IAccountDimensionRepository _accountDimensionRepository;

        public AccountDimensionSearchQueryHandler(IAccountDimensionRepository accountDimensionRepository)
        {
            _accountDimensionRepository = accountDimensionRepository;
        }

        public async Task<ResponseResult<PaginatedList<AccountDimensionSearchDTO>>> Handle(SearchAccountDimensionParamQuery request,
            CancellationToken cancellationToken)
        {
            var results =
               await _accountDimensionRepository.GetSearchAccountDimensionAsync(
               request?.RoleId,
               request.PageNumber,
               request.PageSize,
               request.SortField!,
               request.SortOrder!,
               cancellationToken);

            return results == null ?
                  ResponseResult<PaginatedList<AccountDimensionSearchDTO>>
                  .NotFound(MessageConstants.Message("Permissions", MessageOperationType.NotFound)) :

                  ResponseResult<PaginatedList<AccountDimensionSearchDTO>>
                  .SuccessRetrieveRecords(results);
        }
    }
}