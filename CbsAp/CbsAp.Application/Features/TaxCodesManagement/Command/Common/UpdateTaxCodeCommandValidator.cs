using FluentValidation;

namespace CbsAp.Application.Features.TaxCodes.Command.Common
{
    public class UpdateTaxCodeCommandValidator : AbstractValidator<UpdateTaxCodeCommand>
    {
        public UpdateTaxCodeCommandValidator()
        {
            RuleFor(taxcode => taxcode.EntityID)
              .NotNull().GreaterThan(0)
              .WithMessage("Entity is required.");

            RuleFor(taxcode => taxcode.TaxCodeName)
                .NotEmpty()
                .WithMessage("Tax Code Name is required");

            RuleFor(taxcode => taxcode.TaxRate)
                .NotNull()
                .WithMessage("Tax Rate is required");
        }
    }
}