using FluentValidation;

namespace CbsAp.Application.FakeStoreData.FakeInvoices.InvoiceActions
{
    public class CreateFakeInvoiceCommandValidator : AbstractValidator<CreateFakeInvoiceCommand>
    {
        public CreateFakeInvoiceCommandValidator()
        {
        }
    }
}