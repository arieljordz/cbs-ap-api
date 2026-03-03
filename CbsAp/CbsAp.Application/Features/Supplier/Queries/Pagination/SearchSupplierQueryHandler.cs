using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Supplier;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Supplier.Queries.Pagination
{
    public class SearchSupplierQueryHandler : IQueryHandler<SearchSupplierQuery, ResponseResult<PaginatedList<SupplierSearchDto>>>
    {
        private readonly ISupplierService _supplierService;

        // private rea
        public SearchSupplierQueryHandler(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<ResponseResult<PaginatedList<SupplierSearchDto>>> Handle(SearchSupplierQuery request, CancellationToken cancellationToken)
        {
            var result = await _supplierService.SearchSupplierWithPagination(
                request.EntityName,
                request.SupplierID,
                request.SupplierName,
                request.IsActive,
                request.pageNumber,
                request.pageSize,
                request.sortField,
                request.sortOrder,
                cancellationToken
                );

            return result == null ?
                 ResponseResult<PaginatedList<SupplierSearchDto>>
                .NotFound(MessageConstants.Message("Supplier", MessageOperationType.NotFound)) :
                ResponseResult<PaginatedList<SupplierSearchDto>>
                .SuccessRetrieveRecords(result);
        }
    }
}