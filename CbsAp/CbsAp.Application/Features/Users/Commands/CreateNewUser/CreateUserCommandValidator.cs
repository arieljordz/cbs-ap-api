using FluentValidation;
using FluentValidation.Validators;

namespace CbsAp.Application.Features.Users.Commands.CreateNewUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
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

            RuleFor(u => u.userDTO.UserRoles)
                .Must(list => list.Count > 0)
                .WithMessage(" must contain 1 role assigned");
        }
    }
}