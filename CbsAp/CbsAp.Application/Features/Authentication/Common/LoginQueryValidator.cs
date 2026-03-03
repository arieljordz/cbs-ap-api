using CbsAp.Application.Features.Authentication.Queries;
using FluentValidation;

namespace CbsAp.Application.Features.Authentication.Common
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(req => req.Username)
                .NotEmpty()
                .WithMessage("Username is required");
            RuleFor(req => req.Password)
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }
}