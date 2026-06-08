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

namespace CbsAp.Application.Features.PO.Queries.Reports
{
    public class ExportPOSearchQueryHandler : 
        IQueryHandler<ExportPOSearchQuery, ResponseResult<byte[]>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IExcelService _excelService;
        public ExportPOSearchQueryHandler(IPurchaseOrderRepository purchaseOrderRepository,
            IExcelService excelService)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _excelService = excelService;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportPOSearchQuery request, CancellationToken cancellationToken)
        {
            var result = await _purchaseOrderRepository.ExportPoSearch(
                request.EntityName,
                request.PONo,
                request.Supplier,
                request.IsActive,
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
