using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Supplier.Queries.GetSupplierInfoByID
{
    public record GetSupplierInfoByIDQuery(long supplierInfoID) 
        : IQuery<ResponseResult<SupplierInfoDto>>
    {
    }
}
