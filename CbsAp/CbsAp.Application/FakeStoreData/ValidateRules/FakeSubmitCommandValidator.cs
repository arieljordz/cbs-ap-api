using FluentValidation;

namespace CbsAp.Application.FakeStoreData.ValidateRules
{
    public class fakeSubmitCommandValidator : AbstractValidator<FakeSubmitCommand>
    {
        public fakeSubmitCommandValidator()
        {
            RuleFor(x => x.InvoiceID)
                .NotEmpty()
                .WithMessage("Invoice ID is required");
        }
    }
}