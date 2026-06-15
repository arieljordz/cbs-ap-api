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

namespace CbsAp.Application.Features.PO.Queries.BatchListPurchaseOrder
{
    public class BatchListPurchaseOrderQueryHandler : IQueryHandler<BatchListPurchaseOrderQuery, ResponseResult<PaginatedList<BatchListPurchaseOrderDto>>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public BatchListPurchaseOrderQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<ResponseResult<PaginatedList<BatchListPurchaseOrderDto>>> Handle(BatchListPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            var results = await _purchaseOrderRepository.BatchListPurchaseOrder(
                request.EntityName,
                request.PONo,
                request.Supplier,
                request.IsActive,
                request.GoodReceipt,
                request.pageNumber,
                request.pageSize,
                request.sortField,
                 request.sortOrder,
                cancellationToken
                );
            return results == null ?
               ResponseResult<PaginatedList<BatchListPurchaseOrderDto>>.NotFound("No Search PO found") :
               ResponseResult<PaginatedList<BatchListPurchaseOrderDto>>.SuccessRetrieveRecords(results);
        }
    }
}