using FluentValidation;

namespace CbsAp.Application.Features.Locking.Command
{
    public class ReleaseLockCommandValidator : AbstractValidator<ReleaseLockCommand>
    {
        public ReleaseLockCommandValidator()
        {
            RuleFor(l => l.Id)
               .NotEmpty()
               .WithMessage("ID is required");

        }
    }
}