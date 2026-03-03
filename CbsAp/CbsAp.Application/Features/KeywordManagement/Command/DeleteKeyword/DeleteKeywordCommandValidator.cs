using FluentValidation;

namespace CbsAp.Application.Features.KeywordManagement.Command
{
    public class DeleteKeywordCommandValidator: AbstractValidator<DeleteKeywordCommand>
    {
        public DeleteKeywordCommandValidator()
        {
            RuleFor(e => e.KeywordID)
              .NotEmpty()
              .WithMessage("Keyword ID is required");
        }
    }
}