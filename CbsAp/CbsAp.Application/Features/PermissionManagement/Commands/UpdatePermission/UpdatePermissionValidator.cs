using FluentValidation;

namespace CbsAp.Application.Features.PermissionManagement.Commands.UpdatePermission
{
    public class UpdatePermissionValidator : AbstractValidator<UpdatePermissionCommand>
    {
        public UpdatePermissionValidator()
        {
            RuleFor(p => p.updatePermissionDTO.PermissionName)
             .NotEmpty()
             .WithMessage("Permission Name is required");
        }
    }
}