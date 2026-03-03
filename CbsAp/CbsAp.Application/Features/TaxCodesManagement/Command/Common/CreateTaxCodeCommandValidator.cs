using FluentValidation;

namespace CbsAp.Application.Features.TaxCodes.Command.Common
{
    public class CreateTaxCodeCommandValidator : AbstractValidator<CreateTaxCodeCommand>
    {
        public CreateTaxCodeCommandValidator()
        {
            RuleFor(taxcode => taxcode.EntityID)
               .NotNull().GreaterThan(0)
               .WithMessage("Entity is required.");

            RuleFor(taxcode => taxcode.TaxCodeName)
                .NotEmpty()
                .WithMessage("Tax Code Name is required");

            RuleFor(taxcode => taxcode.TaxRate)
                .NotNull()
                .WithMessage("Tax Rate is required.");
        }
    }
}