using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Abstractions.Services.Supplier;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Supplier.Queries.Reports
{
    public class SupplierExportQueryHandler : IQueryHandler<SupplierExportQuery, ResponseResult<byte[]>>
    {
        private readonly ISupplierService _supplierService;
        private readonly IExcelService _excelService;

        public SupplierExportQueryHandler(ISupplierService supplierService, IExcelService excelService)
        {
            _supplierService = supplierService;
            _excelService = excelService;
        }

        public async Task<ResponseResult<byte[]>> Handle(SupplierExportQuery request, CancellationToken cancellationToken)
        {
            var result = await _supplierService.ExportSupplierToExcel(
                request.EntityName,
                request.SupplierID,
                request.SupplierName,
                request.IsActive,
                cancellationToken);

            var excelBytes = await Task.Run(() => _excelService.GenerateExcel(result, "Supplier"));

            return ResponseResult<byte[]>.Success(excelBytes, "Export supplier excel data");

        }
    }
}
