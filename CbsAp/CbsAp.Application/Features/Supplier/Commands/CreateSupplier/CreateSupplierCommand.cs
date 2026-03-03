using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Supplier.Commands.CreateSupplier
{
    public record CreateSupplierCommand(SupplierInfoDto Supplier, string CreatedBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}
