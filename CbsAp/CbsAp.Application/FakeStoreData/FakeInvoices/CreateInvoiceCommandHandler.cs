using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.FakeStoreData.InvoiceValildation;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;

namespace CbsAp.Application.FakeStoreData.FakeInvoices
{
    public class CreateInvoiceCommandHandler : ICommandHandler<CreateInvoiceCommand, ResponseResult<int>>
    {
        private readonly IUnitofWork _unitOfWork;

        public CreateInvoiceCommandHandler(IUnitofWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public async Task<ResponseResult<int>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            // var invoices = new List<Invoice>(request.requestCount);

            //for (int i = 0; i < request.requestCount; i++)
            //{
            //    var invoice = InvoiceFakeGenerator.GenerateInvoice();

            //    // Wire reverse relationships
            //    foreach (var alloc in invoice.InvoiceAllocationLines!)
            //    {
            //        alloc.Invoice = invoice;

            //        if (alloc.FreeFields != null)
            //        {
            //            foreach (var freeField in alloc.FreeFields)
            //            {
            //                // Use the correct navigation property name here:
            //                freeField.AllocationLine = alloc;
            //            }
            //        }
            //        if (alloc.Dimensions != null)
            //        {
            //        }
            //    }

            //    invoices.Add(invoice);
            //}

            var invoices = PotentiallnvDuplicatesGenerator.GenerateInvoice();
            var invRepo = _unitOfWork.GetRepository<Invoice>();
            invoices.SetAuditFieldsOnCreate("Admin");
            await invRepo.AddRangeAsync(invoices);

            await _unitOfWork.SaveChanges("Admin","import",cancellationToken);
            return ResponseResult<int>.OK(invoices.Count);
        }
    }
}