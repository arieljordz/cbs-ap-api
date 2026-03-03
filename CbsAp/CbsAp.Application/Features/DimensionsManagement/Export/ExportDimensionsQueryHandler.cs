using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.DimensionsManagement;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Dimensions;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.DimensionsManagement.Export
{
    public class ExportDimensionsQueryHandler : IQueryHandler<ExportDimensionsQuery, ResponseResult<byte[]>>
    {
        private readonly IExcelService _excelService;
        private readonly IDimensionRepository _dimensionRepository;

        public ExportDimensionsQueryHandler(IExcelService excelService, IDimensionRepository dimensionRepository)
        {
            _excelService = excelService;
            _dimensionRepository = dimensionRepository;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportDimensionsQuery request, CancellationToken cancellationToken)
        {
            ExpressionStarter<Dimension> predicate = PredicateBuilder.New<Dimension>(true);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(request.EntityName), d => d.EntityProfile != null && d.EntityProfile.EntityName.Contains(request.EntityName!))
                .AndIf(!string.IsNullOrEmpty(request.Dimension), d => d.DimensionCode.Contains(request.Dimension!))
                .AndIf(!string.IsNullOrEmpty(request.Name), d => d.Name.Contains(request.Name!))
                .AndIf(request.Active.HasValue, d => d.IsActive == request.Active!.Value);

            var dimensions = await _dimensionRepository
                .GetDimensionsAsQueryable()
                .Include(d => d.EntityProfile)
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate)
                .Select(d => new DimensionExportDto
                {
                    Entity = d.EntityProfile != null ? d.EntityProfile.EntityName : string.Empty,
                    Dimension = d.DimensionCode,
                    DimensionName = d.Name,
                    ActiveStatus = d.IsActive ? "Active" : "Inactive"
                })
                .ToListAsync(cancellationToken);

            if (dimensions == null || dimensions.Count == 0)
            {
                return ResponseResult<byte[]>.NotFound(
                    MessageConstants.Message("Dimension", MessageOperationType.NotFound));
            }

            var excelBytes = await Task.Run(
                () => _excelService.GenerateExcel(dimensions, "Dimensions"),
                cancellationToken);

            return ResponseResult<byte[]>.Success(excelBytes, "Export excel data");
        }
    }
}
