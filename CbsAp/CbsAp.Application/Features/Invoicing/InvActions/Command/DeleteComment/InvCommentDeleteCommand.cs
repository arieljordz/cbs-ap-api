using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;



namespace CbsAp.Application.Features.Invoicing.InvActions.Command.DeleteComment
{
    public record InvCommentDeleteCommand(LoadInvoiceCommentsDto Dto)
    : ICommand<ResponseResult<bool>>
    {
    }
}