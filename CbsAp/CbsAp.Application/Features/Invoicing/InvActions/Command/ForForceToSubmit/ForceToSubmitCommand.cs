using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ForForceToSubmit
{
    public record ForceToSubmitCommand(InvStatusChangeDto dto, string UpdatedBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}
