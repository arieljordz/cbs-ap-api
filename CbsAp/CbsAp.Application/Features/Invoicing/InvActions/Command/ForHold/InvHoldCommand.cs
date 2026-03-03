using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ForHold
{
    public record InvHoldCommand(
        InvStatusChangeDto dto,
        string UpdatedBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}