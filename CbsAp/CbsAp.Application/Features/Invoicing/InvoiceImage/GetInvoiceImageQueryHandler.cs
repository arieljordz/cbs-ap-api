
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Domain.Entities.Invoicing;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Invoicing.InvoiceImage
{
    public class GetInvoiceImageQueryHandler : IQueryHandler<GetInvoiceImageQuery, string>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ISystemVariableRepository _systemVariableRepository;

        public GetInvoiceImageQueryHandler(IUnitofWork unitofWork, ISystemVariableRepository systemVariableRepository)
        {
            _unitofWork = unitofWork;
            _systemVariableRepository = systemVariableRepository;
        }

        public async Task<string> Handle(GetInvoiceImageQuery request, CancellationToken cancellationToken)
        {
            var invoiceRepository = _unitofWork.GetRepository<Invoice>();

            var imageID = invoiceRepository.Query()
                .Where(i => i.InvoiceID == request.InvoiceID)
                .Select(i => i.ImageID)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(imageID))
            {
                //TODO: return pdf with empty image
                return string.Empty;
            }

            var imagePath = await GetImagePathFromImageId(imageID);

            if (!File.Exists(imagePath))
            {
                //TODO: return pdf with empty image
                return string.Empty;
            }

            return imagePath;
        }

        private async Task<string> GetImagePathFromImageId(string imageId)
        {
            const int expectedLength = 16;
            if (string.IsNullOrWhiteSpace(imageId) || imageId.Length != expectedLength)
                return string.Empty;

            // Extract prefix to fetch base path
            string prefix = imageId.Substring(0, 4);
            string? basePath = await _systemVariableRepository.Query().AsNoTracking()
                .Where(s => s.Name == prefix)
                .Select(s => s.Value)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(basePath))
                return string.Empty;

            // Construct relative path
            string subfolder1 = imageId.Substring(5, 2); // rootfolder - 1st to 2nd segment from imageId
            string subfolder2 = imageId.Substring(7, 3); // subfolder - next 3 segment from imageId
            string fileName = imageId.Substring(5, 8) + ".pdf"; // fileName 

            return Path.Combine(basePath, subfolder1, subfolder2, fileName);
        }
    }
}
