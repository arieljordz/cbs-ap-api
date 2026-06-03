using FluentValidation;



namespace CbsAp.Application.Features.Invoicing.InvActions.Command.DeleteComment
{
    public class InvCommentDeleteCommandValidator : AbstractValidator<InvCommentDeleteCommand>
    {
        public InvCommentDeleteCommandValidator()
        {
            RuleFor(i => i.Dto.InvoiceCommentID)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Comment ID is required");
        }
    }
}