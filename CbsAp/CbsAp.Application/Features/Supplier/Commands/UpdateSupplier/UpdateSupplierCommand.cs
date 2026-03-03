using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Supplier.Commands.UpdateSupplier
{
    public record UpdateSupplierCommand(SupplierInfoDto Supplier, string UpdatedBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}
