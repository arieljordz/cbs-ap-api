using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.Application.Features.Invoicing.InvAttachments.Queries.Download
{
    public record DownloadAttachmentQuery(long InvoiceAttachnmentID) : IQuery<ResponseResult<FileResult>>
    {
    }
}