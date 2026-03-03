using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.AddComment
{
    public record InvCommentCommand(InvoiceCommentDto Dto, string CreatedBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}
