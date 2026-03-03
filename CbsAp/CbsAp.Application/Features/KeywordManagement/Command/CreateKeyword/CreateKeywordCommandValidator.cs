using FluentValidation;

namespace CbsAp.Application.Features.KeywordManagement.Command
{
    public class CreateKeywordCommandValidator : AbstractValidator<CreateKeywordCommand>
    {
        public CreateKeywordCommandValidator()
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