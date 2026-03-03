using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.GoodsReceiptsManagement;
using CbsAp.Application.Features.GoodReceiptsManagement.Queries.Common;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.GoodReceiptsManagement.Queries
{
    public class GetGoodsReceiptsQueryHandler : IQueryHandler<GetGoodsReceiptsQuery, ResponseResult<PaginatedList<GoodsReceiptDTO>>>
    {
        private readonly IGoodsReceiptRepository _goodsReceiptRepository;

        public GetGoodsReceiptsQueryHandler(IGoodsReceiptRepository goodsReceiptRepository)
        {
            _goodsReceiptRepository = goodsReceiptRepository;
        }

        public async Task<ResponseResult<PaginatedList<GoodsReceiptDTO>>> Handle(GetGoodsReceiptsQuery request, CancellationToken cancellationToken)
        {
            ExpressionStarter<GoodReceipt> predicate = PredicateBuilder.New<GoodReceipt>(true);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(request.Entity), gr => gr.EntityProfile != null && gr.EntityProfile.EntityName.Contains(request.Entity!))
                .AndIf(!string.IsNullOrEmpty(request.Supplier), gr => gr.Supplier != null && gr.Supplier.SupplierName != null && gr.Supplier.SupplierName.Contains(request.Supplier!))
                .AndIf(!string.IsNullOrEmpty(request.GoodsReceiptNumber), gr => gr.GoodsReceiptNumber.Contains(request.GoodsReceiptNumber!))
                .AndIf(request.Active.HasValue, gr => gr.Active == request.Active!.Value);

            if (request.DeliveryDateFrom.HasValue)
            {
                var deliveryDateFrom = request.DeliveryDateFrom.Value;
                predicate = predicate.And(gr => gr.DeliveryDate.HasValue && gr.DeliveryDate.Value >= deliveryDateFrom);
            }

            if (request.DeliveryDateTo.HasValue)
            {
                var deliveryDateTo = request.DeliveryDateTo.Value;
                predicate = predicate.And(gr => gr.DeliveryDate.HasValue && gr.DeliveryDate.Value <= deliveryDateTo);
            }

            var goodsReceiptQuery = _goodsReceiptRepository
                .GetGoodsReceiptsAsQueryable()
                .Include(gr => gr.EntityProfile)
                .Include(gr => gr.Supplier)
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(request.SortField))
            {
                goodsReceiptQuery = goodsReceiptQuery.OrderByDescending(gr => gr.LastUpdatedDate ?? gr.CreatedDate);
            }

            var projectedQuery = goodsReceiptQuery.Select(gr => new GoodsReceiptDTO
            {
                GoodsReceiptID = gr.GoodsReceiptID,
                Entity = gr.EntityProfile != null ? gr.EntityProfile.EntityName : string.Empty,
                Supplier = gr.Supplier != null && gr.Supplier.SupplierName != null ? gr.Supplier.SupplierName : string.Empty,
                GoodsReceiptNumber = gr.GoodsReceiptNumber,
                DeliveryNote = gr.DeliveryNote ?? string.Empty,
                DeliveryDate = gr.DeliveryDate,
                Active = gr.Active
            });

            var paginatedGoodsReceipts = await projectedQuery.OrderByDynamic(request.SortField, request.SortOrder)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            return paginatedGoodsReceipts == null
                ? ResponseResult<PaginatedList<GoodsReceiptDTO>>.NotFound(MessageConstants.Message("Goods Receipt", MessageOperationType.NotFound))
                : ResponseResult<PaginatedList<GoodsReceiptDTO>>.SuccessRetrieveRecords(paginatedGoodsReceipts);
        }
    }
}
