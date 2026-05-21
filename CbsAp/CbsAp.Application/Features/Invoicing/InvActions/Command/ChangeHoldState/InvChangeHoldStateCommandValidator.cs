using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ChangeHoldState
{
    public class InvChangeHoldStateCommandValidator : AbstractValidator<InvChangeHoldStateCommand>
    {
        public InvChangeHoldStateCommandValidator()
        {
            RuleFor(i => i.dto.InvoiceID)
               .NotEmpty()
               .WithMessage("Invoice ID is required");
        }
    }
}