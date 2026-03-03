using FluentValidation;

namespace CbsAp.Application.Features.Roles.Command.UpdateRole
{
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleValidator()
        {
            RuleFor(role => role.UpdateRoleDTO.RoleName)
               .NotEmpty()
               .WithMessage("Role Name is required");
        }
    }
}