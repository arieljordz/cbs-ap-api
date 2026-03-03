using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CbsAp.Application.Features.Invoicing.InvAttachments.Queries.Download
{
    public class DownloadAttachmentQueryHandler : IQueryHandler<DownloadAttachmentQuery, ResponseResult<FileResult>>
    {
        private readonly IUnitofWork _unitofWork;

        private readonly IOptions<AppSettings> _storage;

        public DownloadAttachmentQueryHandler(IUnitofWork unitofWork, IOptions<AppSettings> storage)
        {
            _unitofWork = unitofWork;
            _storage = storage;
        }

        public async Task<ResponseResult<FileResult>> Handle(DownloadAttachmentQuery request, CancellationToken cancellationToken)
        {
            var invAttachmentRepo = _unitofWork.GetRepository<InvoiceAttachnment>();
            var attachment = await invAttachmentRepo
                .Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.InvoiceAttachnmentID == request.InvoiceAttachnmentID);

            if (attachment == null)
                return ResponseResult<FileResult>.BadRequest("Invoice Attachment not found");

            var filePath = Path.Combine(_storage.Value.InvAttachmentStoragePath!, attachment.StorageFileName!);

            if (!File.Exists(filePath))
                return ResponseResult<FileResult>.BadRequest("Invoice Attachment File not found");

            var bytes = await File.ReadAllBytesAsync(filePath, cancellationToken);
            var file = new FileContentResult(bytes, attachment.FileType ?? "application/octet-stream")
            {
                FileDownloadName = attachment.StorageFileName
            };

            return ResponseResult<FileResult>.Success(file);
        }
    }
}