using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.AddComment
{
    public class InvCommentCommandValidator : AbstractValidator<InvCommentCommand>
    {
        public InvCommentCommandValidator()
        {
            RuleFor(i => i.Dto.InvoiceID)
             .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Invoice ID is required");

            RuleFor(i => i.Dto.Comment)
             .NotEmpty()
            .WithMessage("Invoice Comment is required");
        }
    }
}
