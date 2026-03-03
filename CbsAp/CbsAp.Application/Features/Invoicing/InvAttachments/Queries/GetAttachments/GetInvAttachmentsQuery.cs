using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvAttachments.Queries.GetAttachments
{
    public record GetInvAttachmentsQuery(long InvoiceID) : IQuery<ResponseResult<List<InvAttachmentDto>>>
    {
    }
}