using FluentValidation;

namespace CbsAp.Application.Features.Locking.Command
{
    public class AcquireLockCommandValidator : AbstractValidator<AcquireLockCommand>
    {
        public AcquireLockCommandValidator()
        {
            RuleFor(l => l.Id)
               .NotEmpty()
               .WithMessage("ID is required");

        }
    }
}