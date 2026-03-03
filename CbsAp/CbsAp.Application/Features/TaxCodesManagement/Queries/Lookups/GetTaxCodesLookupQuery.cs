using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.TaxCodesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.TaxCodesManagement.Queries.Lookups
{
    public record GetTaxCodesLookupQuery() 
        : IQuery<ResponseResult<IEnumerable< TaxCodeLookupDto>>>
    {
    }
}
