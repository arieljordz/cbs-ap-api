using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.TaxCodesManagement;
using CbsAp.Application.Features.Dashboard.Queries.Common;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Enums;

using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Dashboard.Queries
{
    public class GetTaxCodesQueryHandler : IQueryHandler<GetTaxCodesQuery, ResponseResult<PaginatedList<TaxCodeDTO>>>
    {
        private readonly ITaxCodeRepository _taxCodeRepository;

        public GetTaxCodesQueryHandler(ITaxCodeRepository taxCodeRepository)
        {
            _taxCodeRepository = taxCodeRepository;
        }

        public async Task<ResponseResult<PaginatedList<TaxCodeDTO>>> Handle(GetTaxCodesQuery request, CancellationToken cancellationToken)
        {
            ExpressionStarter<TaxCode> predicate =
                PredicateBuilder.New<TaxCode>(true);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(request.EntityName), s => s.EntityProfile!.EntityName.Contains(request.EntityName!))
                .AndIf(!string.IsNullOrEmpty(request.TaxCodeName), s => s.TaxCodeName!.Contains(request.TaxCodeName!))
                .AndIf(!string.IsNullOrEmpty(request.TaxCode), s => s.Code!.Contains(request.TaxCode!));

            var taxCodeQuery = _taxCodeRepository
                .GetTaxCodesAsQueryableAsync()
                .Include(tc => tc.EntityProfile)
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(request.SortField))
            {
                taxCodeQuery = taxCodeQuery.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var paginatedTaxCodeQuery = taxCodeQuery.Select(s => new TaxCodeDTO
            {
                TaxCodeID = s.TaxCodeID,
                EntityID = s.EntityID,
                EntityName = s.EntityProfile.EntityName,
                TaxCodeName = s.TaxCodeName ?? string.Empty,
                Code = s.Code ?? string.Empty,
                TaxRate = s.TaxRate
            });

            var taxCodePagination = await paginatedTaxCodeQuery.OrderByDynamic(request.SortField, request.SortOrder)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            return taxCodePagination == null ? ResponseResult<PaginatedList<TaxCodeDTO>>.NotFound(MessageConstants.Message("TaxCode", MessageOperationType.NotFound)) :
                    ResponseResult<PaginatedList<TaxCodeDTO>>.SuccessRetrieveRecords(taxCodePagination);
        }
    }
}