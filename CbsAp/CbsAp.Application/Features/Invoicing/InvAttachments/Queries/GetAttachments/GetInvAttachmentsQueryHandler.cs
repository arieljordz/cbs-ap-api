using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CbsAp.Application.Features.Invoicing.InvAttachments.Queries.GetAttachments
{
    public class GetInvAttachmentsQueryHandler : IQueryHandler<GetInvAttachmentsQuery, ResponseResult<List<InvAttachmentDto>>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetInvAttachmentsQueryHandler(IUnitofWork unitofWork, IOptions<AppSettings> storage)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<List<InvAttachmentDto>>> Handle(GetInvAttachmentsQuery request, CancellationToken cancellationToken)
        {
            var invAttachmentRepo = _unitofWork.GetRepository<InvoiceAttachnment>();
            var attachment = await invAttachmentRepo
                .Query()
                .AsNoTracking()
                .Where(i => i.InvoiceID == request.InvoiceID)
                .OrderByDescending(i => i.CreatedDate)
                .Select(
                 i => new InvAttachmentDto
                 {
                     InvoiceAttachnmentID = i.InvoiceAttachnmentID,
                     InvoiceID = i.InvoiceID,
                     FileType = i.FileType,
                     StorageFileName = i.StorageFileName,
                     OriginalFileName = i.OriginalFileName,
                 })
                  .ToListAsync();

            if (attachment == null)
                return ResponseResult<List<InvAttachmentDto>>.BadRequest("Invoice Attachment not found");

            return ResponseResult<List<InvAttachmentDto>>.Success(attachment);
        }
    }
}