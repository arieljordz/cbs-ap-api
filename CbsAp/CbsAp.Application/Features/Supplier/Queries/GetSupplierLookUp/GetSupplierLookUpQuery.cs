using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Supplier.Queries.GetSupplierLookUp
{
    public record GetSupplierLookUpQuery() : IQuery<ResponseResult<IEnumerable<SupplierLookUpDto>>>
    {
    }
}