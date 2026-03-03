using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.TaxCodesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.TaxCodesManagement.Queries.Common
{
    public record GetTaxCodeByIDQuery(long taxCodeId) : IQuery<ResponseResult<TaxCodeDTO>>
    {
    }
}
