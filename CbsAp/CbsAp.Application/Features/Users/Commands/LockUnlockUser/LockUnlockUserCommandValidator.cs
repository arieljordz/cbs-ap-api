using FluentValidation;

namespace CbsAp.Application.Features.Users.Commands.LockUnlockUser
{
    public class LockUnlockUserCommandValidator : AbstractValidator<LockUnlockUserCommand>
    {
        public LockUnlockUserCommandValidator()
        {
            RuleFor(l => l.UserAccountID)
               .NotEmpty()
               .WithMessage("User Account Id is required");
        }
    }
}
