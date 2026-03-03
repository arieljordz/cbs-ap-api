using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.TaxCodesManagement;
using CbsAp.Application.Features.TaxCodesManagement.Queries.Common;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.TaxCodesManagement.Queries
{
    public class ExportTaxCodesQueryHandler : IQueryHandler<ExportTaxCodesQuery, ResponseResult<byte[]>>
    {
        private readonly ITaxCodeRepository _taxCodeRepository;
        private readonly IExcelService _excelService;

        public ExportTaxCodesQueryHandler(ITaxCodeRepository taxCodeRepository, IExcelService excelService)
        {
            _taxCodeRepository = taxCodeRepository;
            _excelService = excelService;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportTaxCodesQuery request, CancellationToken cancellationToken)
        {
            ExpressionStarter<TaxCode> predicate = PredicateBuilder.New<TaxCode>(true);

            if (!string.IsNullOrEmpty(request.EntityName))
            {
                predicate = predicate.And(tc => tc.EntityProfile != null &&
                                                tc.EntityProfile.EntityName != null &&
                                                tc.EntityProfile.EntityName.Contains(request.EntityName));
            }

            if (!string.IsNullOrEmpty(request.TaxCodeName))
            {
                predicate = predicate.And(tc => tc.TaxCodeName != null &&
                                                tc.TaxCodeName.Contains(request.TaxCodeName));
            }

            if (!string.IsNullOrEmpty(request.Code))
            {
                predicate = predicate.And(tc => tc.Code != null &&
                                                tc.Code.Contains(request.Code));
            }

            var taxCodeQuery = await _taxCodeRepository
                .GetTaxCodesAsQueryableAsync()
                .Include(tc => tc.EntityProfile)
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate).Select(tc => new TaxCodeExportDto
                {
                    EntityName = tc.EntityProfile.EntityName,
                    TaxCodeName = tc.TaxCodeName ?? string.Empty,
                    Code = tc.Code ?? string.Empty,
                    TaxRate = tc.TaxRate
                }).ToListAsync(cancellationToken: cancellationToken);

            if (taxCodeQuery.Count == 0 || taxCodeQuery == null)
            {
                return ResponseResult<byte[]>.NotFound(MessageConstants.Message("Tax Code", MessageOperationType.NotFound));
            }

            var excelBytes = await Task.Run(() => _excelService.GenerateExcel(taxCodeQuery, "Tax Code"));

            return ResponseResult<byte[]>.Success(excelBytes, "Export excel data");
        }
    }
}
