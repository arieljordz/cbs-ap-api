using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.PO.Queries.ReportDetail
{
    public class ExportPODetailSearchQueryHandler : 
        IQueryHandler<ExportPODetailSearchQuery, ResponseResult<byte[]>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IExcelService _excelService;
        public ExportPODetailSearchQueryHandler(IPurchaseOrderRepository purchaseOrderRepository,
            IExcelService excelService)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _excelService = excelService;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportPODetailSearchQuery request, CancellationToken cancellationToken)
        {
            var result = await _purchaseOrderRepository.ExportPoDetailSearch(
                request.PurchaseOrderId,
                request.SearchLine,
                cancellationToken);

            if (result.Count == 0 || result == null)
            {
                return ResponseResult<byte[]>.NotFound(MessageConstants.Message("ExceptionQueue", MessageOperationType.NotFound));
            }
            var excelBytes = await Task.Run(() => _excelService.GenerateExcel(result, "PurchaseOrder", true));

            return ResponseResult<byte[]>.Success(excelBytes, "Export Purchase orders excel data");
        }
    }
}
