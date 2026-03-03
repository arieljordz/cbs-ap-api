using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvoiceImage
{
    public class GetInvoiceImageQueryValidator : AbstractValidator<GetInvoiceImageQuery>
    {
        public GetInvoiceImageQueryValidator()
        {
            RuleFor(x => x.InvoiceID)
                .NotEmpty().WithMessage("Invoice ID is required.")
                .GreaterThan(0).WithMessage("Invoice ID must be greater than zero.");
        }
    }
}
