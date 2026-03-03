using FluentValidation;

namespace CbsAp.Application.FakeStoreData.FakeInvoices.InvoiceArchiveActions
{
    public class CreateFakeInvoiceArchiveCommandValidator : AbstractValidator<CreateFakeInvoiceArchiveCommand>
    {
        public CreateFakeInvoiceArchiveCommandValidator()
        {
        }
    }
}
