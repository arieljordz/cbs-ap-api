using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Entity;
using CbsAp.Application.Abstractions.Services.PO;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Services.PO
{
    public class PurchaseOrderService : IPurchaseOrderService
    {

        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<PurchaseOrderHeaderDto?> GetPurchaseOrderByIdAsync(long purchaseOrderId)
        {
            var result = await _purchaseOrderRepository.GetPurchaseOrderByID(purchaseOrderId)!;

            return result;
        }

        public async Task<PaginatedList<PurchaseHeaderLineDetailsDto>> GetPurchaseOrderListByIdAsync(long purchaseOrderId,int pageNumber,
        int pageSize,
        string? sortField,
        int? sortOrder,
        string? searchLine,
        CancellationToken token)
        {
            var result = await _purchaseOrderRepository.GetPurchaseOrderListByID(purchaseOrderId, pageNumber, pageSize, sortField, sortOrder, searchLine, token)!;

            return result;
        }

    }
}
