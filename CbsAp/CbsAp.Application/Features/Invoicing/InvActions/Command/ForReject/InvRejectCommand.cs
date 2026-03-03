using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ForReject
{
    public record InvRejectCommand(InvStatusChangeDto dto, string UpdatedBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}