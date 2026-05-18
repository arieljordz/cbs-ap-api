using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.Validate
{
    public class ValidateByIdsCommandValidator : AbstractValidator<ValidateByIdsCommand>
    {
        public ValidateByIdsCommandValidator()
        {
            RuleFor(x => x.InvoiceIds)
                .NotNull()
                .NotEmpty()
                .WithMessage("Invoice IDs are required.");

            RuleForEach(x => x.InvoiceIds)
                .GreaterThan(0)
                .WithMessage("Each Invoice ID must be greater than 0.");
        }
    }
}