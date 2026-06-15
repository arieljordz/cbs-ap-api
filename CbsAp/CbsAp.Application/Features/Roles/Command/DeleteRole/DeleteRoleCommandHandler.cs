using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Roles.Command.DeleteRole
{
    public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ILogger<DeleteRoleCommandHandler> _logger;



        public DeleteRoleCommandHandler(IUnitofWork unitofWork, ILogger<DeleteRoleCommandHandler> logger)
        {
            _unitofWork = unitofWork;
            _logger = logger;
        }


        public async Task<ResponseResult<bool>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var userRoleRepo = _unitofWork.GetRepository<UserRole>();
            var roleRepo = _unitofWork.GetRepository<Role>();

            var roleHasUser = await userRoleRepo.AnyAsync(a => a.RoleID == request.roleID);

            if (roleHasUser)
            {
                return ResponseResult<bool>.BadRequest("Role has existing user(s).");
            }


            var isRoleExists = await roleRepo.AnyAsync(a => a.RoleID == request.roleID);

            if (isRoleExists)
            {
                var role = await roleRepo.GetByIdAsync(request.roleID);
                await roleRepo.DeleteAsync(role);
                await _unitofWork.SaveChanges("", "", cancellationToken);
                return ResponseResult<bool>.OK(MessageConstants.Message("role", MessageOperationType.Delete));
            }
            else
            {
                return ResponseResult<bool>.BadRequest("Role not exists.");
            }
        }
    }
}
