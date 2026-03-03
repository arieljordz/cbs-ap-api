using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Roles.Command.CreateRole
{
    public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly ILogger<CreateRoleCommandHandler> _logger;

        public CreateRoleCommandHandler(IUnitofWork unitOfWork,
            ILogger<CreateRoleCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        public async Task<ResponseResult<string>> Handle(
            CreateRoleCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Adapt<CreateRoleDTO>();
            var roleName = dto.RoleName?.Trim();

            if (string.IsNullOrWhiteSpace(roleName))
            {
                return ResponseResult<string>.BadRequest("Role name is required.");
            }

            var roleRepo = _unitOfWork.GetRepository<Role>();
            if (await roleRepo.AnyAsync(r => r.RoleName.Trim() == roleName))
            {
                return ResponseResult<string>.Confict(
                    MessageConstants.Message(MessageOperationType.Exist, roleName)
                );
            }

            var newRole = dto.Adapt<Role>();

            newRole.SetAuditFieldsOnCreate(request.CreatedBy);

            // Assign related entities through navigation properties
            newRole.RolePermissionGroups = [.. dto.RolePermissionGroups.Select(id => new RolePermissionGroup
            {
                PermissionID = id
            })];

            newRole.UserRoles = [.. dto.UserRoles.Select(userId => new UserRole
            {
                UserAccountID = userId
            })];

            newRole.RoleEntities = [.. dto.RoleEntities.Select(entityId => new RoleEntity
            {
                EntityProfileID = entityId
            })];

            await roleRepo.AddAsync(newRole);

            var saveResult = await _unitOfWork.SaveChanges(cancellationToken);
            if (saveResult)
            {
                return ResponseResult<string>.Created("New role created");
            }

            _logger.LogError(MessageConstants.FormatMessage(MessageConstants.AddError, dto.RoleName!));

            return ResponseResult<string>.BadRequest("Error creating new role", dto.RoleName!);
        }

    }
}
