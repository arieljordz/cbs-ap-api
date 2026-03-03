using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.TaxCode;
using CbsAp.Application.DTOs.TaxCodesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.TaxCodesManagement.Queries.Lookups
{
    public class GetTaxCodesLookupQueryHandler 
        : IQueryHandler<GetTaxCodesLookupQuery, ResponseResult<IEnumerable<TaxCodeLookupDto>>>
    {
        private readonly ITaxcodeService _taxcodeService;

        public GetTaxCodesLookupQueryHandler(ITaxcodeService taxcodeService)
        {
            _taxcodeService = taxcodeService;
        }

        public async Task<ResponseResult<IEnumerable<TaxCodeLookupDto>>> Handle(
            GetTaxCodesLookupQuery request,
            CancellationToken cancellationToken)
        {
            var results = await _taxcodeService.GetTaxCodeLookupAsync();

            return !results.Any() ?
                ResponseResult<IEnumerable<TaxCodeLookupDto>>.NotFound("No tax code  lookup found ")
                :ResponseResult<IEnumerable<TaxCodeLookupDto>>.SuccessRetrieveRecords(results);
        }
    }
}
