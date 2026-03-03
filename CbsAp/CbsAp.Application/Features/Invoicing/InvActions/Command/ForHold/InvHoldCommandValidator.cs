using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ForHold
{
    public class InvHoldCommandValidator : AbstractValidator<InvHoldCommand>
    {
        public InvHoldCommandValidator()
        {
            RuleFor(i => i.dto.InvoiceID)
               .NotEmpty()
               .WithMessage("Invoice ID is required");
        }
    }
}