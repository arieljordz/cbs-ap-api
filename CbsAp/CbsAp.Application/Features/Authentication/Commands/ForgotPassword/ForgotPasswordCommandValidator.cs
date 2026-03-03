using FluentValidation;
using FluentValidation.Validators;

namespace CbsAp.Application.Features.Authentication.Commands.ForgotPassword
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(u => u.EmailAddress)
                .NotEmpty()
                .WithMessage("Email address is required")
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("Please provide a valid email address.");
        }
    }
}