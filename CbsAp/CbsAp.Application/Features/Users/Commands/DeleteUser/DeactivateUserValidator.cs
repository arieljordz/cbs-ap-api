using FluentValidation;

namespace CbsAp.Application.Features.Users.Commands.DeleteUser
{
    public class DeactivateUserValidator : AbstractValidator<DeactivateUserCommand>
    {
        public DeactivateUserValidator()
        {
            RuleFor(u => u.UserAccountID)
              .NotEmpty()
              .WithMessage("UserAccountID is required");
        }
    }
}