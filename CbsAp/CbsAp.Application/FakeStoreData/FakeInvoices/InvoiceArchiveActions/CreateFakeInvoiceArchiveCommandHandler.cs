using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.FakeStoreData;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.InvoicingArchive;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.FakeStoreData.FakeInvoices.InvoiceArchiveActions
{
    public class CreateFakeInvoiceArchiveCommandHandler : ICommandHandler<CreateFakeInvoiceArchiveCommand, ResponseResult<int>>
    {
        private readonly IUnitofWork _unitOfWork;

        public CreateFakeInvoiceArchiveCommandHandler(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseResult<int>> Handle(CreateFakeInvoiceArchiveCommand request, CancellationToken cancellationToken)
        {
            var archiveInvoice = InvoiceFakeGenerator.GenerateArchiveInvoice(request.Dto, request.CreatedBy);

            // Ensure archive defaults in case caller omitted them.
            archiveInvoice.QueueType ??= InvoiceQueueType.ArchiveQueue;
            archiveInvoice.StatusType ??= InvoiceStatusType.Archived;

            var archiveRepo = _unitOfWork.GetRepository<InvoiceArchive>();
            await archiveRepo.AddAsync(archiveInvoice);

            var saved = await _unitOfWork.SaveChanges(cancellationToken);
            if (!saved)
            {
                return ResponseResult<int>.BadRequest("Failed to generate archive invoice.");
            }

            return ResponseResult<int>.OK("Fake archive invoice generated successfully.");
        }
    }
}
