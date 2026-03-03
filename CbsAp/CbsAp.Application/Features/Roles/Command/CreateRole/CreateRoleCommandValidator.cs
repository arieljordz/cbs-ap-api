using FluentValidation;

namespace CbsAp.Application.Features.Roles.Command.CreateRole
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(role => role.roleDTO.RoleName)
                .NotEmpty()
                .WithMessage("Role Name is required");
        }
    }
}