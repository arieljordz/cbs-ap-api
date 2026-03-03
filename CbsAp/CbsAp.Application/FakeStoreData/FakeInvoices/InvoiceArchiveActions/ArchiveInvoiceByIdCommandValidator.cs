using FluentValidation;

namespace CbsAp.Application.FakeStoreData.FakeInvoices.InvoiceArchiveActions
{
    public class ArchiveInvoiceByIdCommandValidator : AbstractValidator<ArchiveInvoiceByIdCommand>
    {
        public ArchiveInvoiceByIdCommandValidator()
        {
            RuleFor(x => x.InvoiceId)
                .GreaterThan(0)
                .WithMessage("Invoice identifier must be greater than zero.");
        }
    }
}
