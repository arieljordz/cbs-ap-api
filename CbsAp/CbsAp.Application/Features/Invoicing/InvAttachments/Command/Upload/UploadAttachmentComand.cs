using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvAttachments.Command.Upload
{
    public record UploadAttachmentComand(InvAttachmentFromDto dto, string CreatedBy) : ICommand<ResponseResult<InvAttachmentDto>>
    {
    }
}