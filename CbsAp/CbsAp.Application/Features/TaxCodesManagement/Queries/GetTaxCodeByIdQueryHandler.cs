using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.TaxCodesManagement;
using CbsAp.Application.Features.TaxCodesManagement.Queries.Common;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.TaxCodesManagement.Queries
{
    public class GetTaxCodeByIdQueryHandler : IQueryHandler<GetTaxCodeByIDQuery, ResponseResult<TaxCodeDTO>>
    {
        private readonly ITaxCodeRepository _taxCodeRepository;

        public GetTaxCodeByIdQueryHandler(ITaxCodeRepository taxCodeRepository)
        {
            _taxCodeRepository = taxCodeRepository;
        }

        public async Task<ResponseResult<TaxCodeDTO>> Handle(GetTaxCodeByIDQuery request, CancellationToken cancellationToken)
        {
            var taxCode = await _taxCodeRepository.GetTaxCodesAsQueryableAsync()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(tc => tc.TaxCodeID == request.taxCodeId);

            if (taxCode == null)
                return ResponseResult<TaxCodeDTO>.BadRequest("Tax code not found");

            var taxCodeDto = new TaxCodeDTO
            {
                TaxCodeID = taxCode.TaxCodeID,
                EntityID = taxCode.EntityID,
                EntityName = taxCode.EntityProfile!.EntityName,
                TaxCodeName = taxCode.TaxCodeName ?? string.Empty,
                Code = taxCode.Code ?? string.Empty,
                TaxRate = taxCode.TaxRate
            };

            return ResponseResult<TaxCodeDTO>.OK(taxCodeDto);

        }
    }
}
