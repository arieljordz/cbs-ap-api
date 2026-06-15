using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CbsAp.Application.Features.Roles.Command.DeleteRole
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator() 
        {
            RuleFor(r => r.roleID)
                .NotEmpty()
                .WithMessage("Role ID is required");
        }
    }
}
