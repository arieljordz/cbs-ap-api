using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.PO.Queries.POSearch
{
    public class POSearchQueryHandler : IQueryHandler<POSearchQuery, ResponseResult<PaginatedList<POSearchDto>>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public POSearchQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<ResponseResult<PaginatedList<POSearchDto>>> Handle(POSearchQuery request, CancellationToken cancellationToken)
        {
            var results = await _purchaseOrderRepository.PoSearch(
                request.EntityName,
                request.PONo,
                request.Supplier,
                request.IsActive,
                request.pageNumber,
                request.pageSize,
                request.sortField,
                 request.sortOrder,
                cancellationToken
                );
            return results == null ?
               ResponseResult<PaginatedList<POSearchDto>>.NotFound("No Search PO found") :
               ResponseResult<PaginatedList<POSearchDto>>.SuccessRetrieveRecords(results);
        }
    }
}