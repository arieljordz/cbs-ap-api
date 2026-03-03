using FluentValidation;

namespace CbsAp.Application.Features.PermissionManagement.Commands.CreatePermission
{
    public class CreatePermissionValidator : AbstractValidator<PermissionCommand>
    {
        public CreatePermissionValidator()
        {
            RuleFor(p => p.CreatePermissionDTO.PermissionName)
           .NotEmpty()
           .WithMessage("Permission Name is required");
        }
    }
}