using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Entity;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Entity.Queries.Reports
{
    public class ExportEntityQueryHandler : IQueryHandler<ExportEntityQuery, ResponseResult<byte[]>>
    {
        private readonly IEntityService _entityService;
        private readonly IExcelService _excelService;

        public ExportEntityQueryHandler(IEntityService entityService, IExcelService excelService)
        {
            _entityService = entityService;
            _excelService = excelService;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportEntityQuery request, CancellationToken cancellationToken)
        {
            var result = await _entityService.ExportEntityToExcel(request.EntityName, request.EntityCode, cancellationToken);

            if (!result.Any() || result == null)
            {
                return ResponseResult<byte[]>.NotFound(MessageConstants.Message("Entity", MessageOperationType.NotFound));
            }

            var excelBytes = await Task.Run(() => _excelService.GenerateExcel(result, "Entity"));

            return ResponseResult<byte[]>.Success(excelBytes, "Export excel data");
        }
    }
}