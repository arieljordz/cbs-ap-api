using FluentValidation;

namespace CbsAp.Application.Features.Authentication.Commands.SetNewPassword
{
    public class SetPasswordCommandValidator : AbstractValidator<SetNewPasswordCommand>
    {
        public SetPasswordCommandValidator()
        {
            RuleFor(a => a.PasswordrecoveryToken)
                .NotEmpty()
                .WithMessage("Invalid request for setting new password.");

            RuleFor(a => a.NewPassword)
                .NotEmpty()
                .WithMessage("Password is required.")
                 .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                 .MaximumLength(20).WithMessage("Password must be at max 20 characters long.")
                 .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                 .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                 .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                 .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        }
    }
}