using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using Microsoft.Extensions.Options;

namespace CbsAp.Application.Features.Invoicing.InvAttachments.Command.Upload
{
    public class UploadAttachmentComandHandler : ICommandHandler<UploadAttachmentComand, ResponseResult<InvAttachmentDto>>
    {
        private readonly IUnitofWork _unitofWork;

        private readonly IOptions<AppSettings> _storage;

        public UploadAttachmentComandHandler(IUnitofWork unitofWork, IOptions<AppSettings> storage)
        {
            _unitofWork = unitofWork;
            _storage = storage;
        }

        public async Task<ResponseResult<InvAttachmentDto>> Handle(UploadAttachmentComand request, CancellationToken cancellationToken)
        {
            var invAttachmentRepo = _unitofWork.GetRepository<InvoiceAttachnment>();

            var file = request.dto.File;

            if (file.Length > 10 * 1024 * 1024)
                return ResponseResult<InvAttachmentDto>.BadRequest("File is Max 5MB");

            var fileExt = Path.GetExtension(file.FileName);
            var now = DateTime.UtcNow;
            var renameOrigFileName = $"{request.dto.InvoiceID}_{now:ddMMyyyy}_{now:HHmmssfff}{fileExt}";
            var savePath = Path.Combine(_storage.Value.InvAttachmentStoragePath!, renameOrigFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

            using var stream = new FileStream(savePath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            var attachment = new InvoiceAttachnment
            {
                OriginalFileName = file.FileName,
                StorageFileName = renameOrigFileName,
                FileType = file.ContentType,
                InvoiceID = request.dto.InvoiceID,
            };

            await invAttachmentRepo.AddAsync(attachment);
            bool isSaved = await _unitofWork.SaveChanges(cancellationToken);

            var dto = new InvAttachmentDto
            {
                InvoiceAttachnmentID = attachment.InvoiceAttachnmentID,
                OriginalFileName = attachment.OriginalFileName,
                StorageFileName = attachment.StorageFileName,
                FileType = attachment.FileType,
                InvoiceID = attachment.InvoiceID,
            };

            if (!isSaved)
                return ResponseResult<InvAttachmentDto>.BadRequest("Error Adding New Invoice  Comments");
            return ResponseResult<InvAttachmentDto>.Success(dto);
        }
    }
}