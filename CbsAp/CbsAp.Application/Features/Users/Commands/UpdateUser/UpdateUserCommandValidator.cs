using FluentValidation;
using FluentValidation.Validators;

namespace CbsAp.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(u => u.userDTO.UserAccountID)
           .NotEmpty()
           .WithMessage("User Account Id is required");
            RuleFor(u => u.userDTO.UserID)
            .NotEmpty()
            .WithMessage("User Id is required");

            RuleFor(u => u.userDTO.FirstName)
               .NotEmpty()
               .WithMessage("First Name is required");

            RuleFor(u => u.userDTO.LastName)
               .NotEmpty()
               .WithMessage("Last Name is required");

            RuleFor(u => u.userDTO.EmailAddress)
              .NotEmpty()
              .EmailAddress(EmailValidationMode.AspNetCoreCompatible);

            RuleFor(u => u.userDTO.Password)
              .NotEmpty()
              .WithMessage("Password is required.")
              .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
              .MaximumLength(20).WithMessage("Password must be at max 20 characters long.")
              .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
              .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
              .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
              .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.")
              .When(x => x.userDTO.PasswordMandatory);

        }
    }
}