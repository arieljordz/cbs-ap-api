using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Invoicing.InvRoutingFlows.Queries.Reports;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Queries.Reports
{
    public class InvRoutingFlowExportQueryHandler : IQueryHandler<InvRoutingFlowExportQuery, ResponseResult<byte[]>>
    {
        private readonly IInvRoutingFlowRepository _invRoutingFlowRepository;

        private readonly IExcelService _excelService;

        public InvRoutingFlowExportQueryHandler(IInvRoutingFlowRepository invRoutingFlowRepository, IExcelService excelService)
        {

            _invRoutingFlowRepository = invRoutingFlowRepository;
            _excelService = excelService;
        }

        public async Task<ResponseResult<byte[]>> Handle(InvRoutingFlowExportQuery request, CancellationToken cancellationToken)
        {
            var result = await _invRoutingFlowRepository.ExportInvRoutingFlowToExcel(
                request.EntityName,
                request.InvRoutingFlowName,
                request.LinkSupplier,
                request.Roles,
                request.MatchReference,
                cancellationToken);

            var excelBytes = await Task.Run(() => _excelService.GenerateExcel(result, "Supplier"));

            return ResponseResult<byte[]>.Success(excelBytes, "Export invoice routing flow excel data");
        }
    }
}
