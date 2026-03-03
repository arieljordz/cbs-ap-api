using FluentValidation;

namespace CbsAp.Application.Features.Authentication.Commands.SwitchRole
{
    public class SwitchRoleCommandValidator : AbstractValidator<SwitchRoleCommand>
    {
        public SwitchRoleCommandValidator()
        {
            RuleFor(a => a)
                .Must(a=>a.RoleID>0)
                .WithMessage("No role defined.");

            RuleFor(a => a.UserName)
                .NotEmpty()
                .WithMessage("Username is required.");
                
        }
    }
}