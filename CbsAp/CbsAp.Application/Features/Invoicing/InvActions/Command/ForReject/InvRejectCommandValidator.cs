using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ForReject
{
    public class InvRejectCommandValidator : AbstractValidator<InvRejectCommand>
    {
        public InvRejectCommandValidator()
        {
            RuleFor(i => i.dto)
                .NotNull()
                .WithMessage("Request payload is required");

            RuleFor(i => i.dto.InvoiceID)
                .NotEmpty()
                .WithMessage("Invoice ID is required");

            RuleFor(i => i.dto.Reason)
                .NotEmpty()
                .WithMessage("Reason is required when rejecting an invoice");
        }
    }
}