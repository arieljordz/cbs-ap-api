using FluentValidation;

namespace CbsAp.Application.Features.KeywordManagement.Command
{
    public class UpdateKeywordCommandValidator : AbstractValidator<UpdateKeywordCommand>
    {
        public UpdateKeywordCommandValidator()
        {
            RuleFor(keyword => keyword.KeywordName)
              .NotEmpty()
              .WithMessage("Keyword is required");

            RuleFor(keyword => keyword.InvoiceRoutingFlowID)
              .NotNull().GreaterThan(0)
              .WithMessage("InvoiceRoutingFlowID is required.");
        }
    }
}