using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.GoodsReceiptsManagement;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.GoodReceiptsManagement.Export
{
    public class ExportGoodsReceiptsQueryHandler : IQueryHandler<ExportGoodsReceiptsQuery, ResponseResult<byte[]>>
    {
        private readonly IExcelService _excelService;
        private readonly IGoodsReceiptRepository _goodsReceiptRepository;

        public ExportGoodsReceiptsQueryHandler(IExcelService excelService, IGoodsReceiptRepository goodsReceiptRepository)
        {
            _excelService = excelService;
            _goodsReceiptRepository = goodsReceiptRepository;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportGoodsReceiptsQuery request, CancellationToken cancellationToken)
        {
            ExpressionStarter<GoodReceipt> predicate = PredicateBuilder.New<GoodReceipt>(true);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(request.EntityName), gr => gr.EntityProfile != null && gr.EntityProfile.EntityName.Contains(request.EntityName!))
                .AndIf(!string.IsNullOrEmpty(request.SupplierName), gr => gr.Supplier != null && gr.Supplier.SupplierName != null && gr.Supplier.SupplierName.Contains(request.SupplierName!))
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

            var goodsReceipts = await _goodsReceiptRepository
                .GetGoodsReceiptsAsQueryable()
                .Include(gr => gr.EntityProfile)
                .Include(gr => gr.Supplier)
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate)
                .Select(gr => new GoodsReceiptExportDto
                {
                    Entity = gr.EntityProfile != null ? gr.EntityProfile.EntityName : string.Empty,
                    Supplier = gr.Supplier != null && gr.Supplier.SupplierName != null ? gr.Supplier.SupplierName : string.Empty,
                    GoodsReceiptNumber = gr.GoodsReceiptNumber,
                    DeliveryNote = gr.DeliveryNote ?? string.Empty,
                    ActiveStatus = gr.Active ? "Yes" : "No",
                    DeliveryDate = gr.DeliveryDate
                })
                .ToListAsync(cancellationToken);

            if (goodsReceipts == null || goodsReceipts.Count == 0)
            {
                return ResponseResult<byte[]>.NotFound(
                    MessageConstants.Message("Goods Receipt", MessageOperationType.NotFound));
            }

            var excelBytes = await Task.Run(
                () => _excelService.GenerateExcel(goodsReceipts, "GoodsReceipts"),
                cancellationToken);

            return ResponseResult<byte[]>.Success(excelBytes, "Export excel data");
        }
    }
}
