using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.PermissionManagement.Reports
{
    public class ExportPermissionQueryHandler : IQueryHandler<ExportPermissionQuery, ResponseResult<byte[]>>
    {
        private readonly IPermissionManagementRepository _permissionManagementRepository;

        private readonly IExcelService _excelService;

        public ExportPermissionQueryHandler(IPermissionManagementRepository permissionManagementRepository, IExcelService excelService)
        {
            _permissionManagementRepository = permissionManagementRepository;
            _excelService = excelService;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportPermissionQuery request, CancellationToken cancellationToken)
        {
            var results = await _permissionManagementRepository
            .ExportExcelPermissionAsync(
            request?.PermissionID,
            request?.PermissionName,
            request?.IsActive,
            cancellationToken);

            if (!results.Any() || results == null)
                return ResponseResult<byte[]>.NotFound(MessageConstants.Message("Permissions Groups", MessageOperationType.NotFound));

            var excelBytes = await Task.Run(() => _excelService.GenerateExcel(results, "PermissionGroups"));

            return ResponseResult<byte[]>.Success(excelBytes, "retrieve excel data");
        }
    }
}