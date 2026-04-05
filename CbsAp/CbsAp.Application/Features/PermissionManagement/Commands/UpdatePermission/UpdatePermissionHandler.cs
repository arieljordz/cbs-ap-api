using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.PermissionManagement.Commands.UpdatePermission
{
    public class UpdatePermissionHandler : ICommandHandler<UpdatePermissionCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ILogger<UpdatePermissionHandler> _logger;

        public UpdatePermissionHandler(IUnitofWork unitofWork,
                                       ILogger<UpdatePermissionHandler> logger)
        {
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public async Task<ResponseResult<string>> Handle(UpdatePermissionCommand request,
                                                 CancellationToken cancellationToken)
        {
            try
            {
                var permissionDTO = request.updatePermissionDTO.Adapt<UpdatePermissionDTO>();
                
                // Fetch the existing permission to preserve fields not in the DTO
                var existingPermission = await _unitofWork.GetRepository<Permission>()
                    .GetByIdAsync(permissionDTO.PermissionID);

                if (existingPermission == null)
                {
                    return ResponseResult<string>.NotFound("Permission not found");
                }

                // Check if permission name already exists (excluding current permission)
                if (await IsPermissionExists(existingPermission.PermissionID, permissionDTO.PermissionName))
                {
                    return ResponseResult<string>.Confict("Permission is already exist");
                }

                // Update only the properties from the DTO, preserving IsActive and other fields
                existingPermission.PermissionName = permissionDTO.PermissionName;
                existingPermission.SetAuditFieldsOnUpdate(request.updatedBy);

                var permissionGroups = request.updatePermissionDTO.GroupPanel
                    .SelectMany(gp => gp.Operations)
                    .Select(op => new PermissionGroup
                    {
                        PermissionID = existingPermission.PermissionID,
                        OperationID = op.OperationID,
                        Access = op.Access,
                    }).ToList();

                await AddOrUpdatePermissionGroupsAsync(permissionGroups);
                await UpdateRolePermissionGroup(permissionDTO.Roles, existingPermission.PermissionID);

                await _unitofWork.GetRepository<Permission>().UpdateAsync(existingPermission.PermissionID, existingPermission);

                if (!await SavePermissionAsync(cancellationToken))
                {
                    _logger.LogError(MessageConstants.FormatMessage(MessageConstants.AddError, existingPermission.PermissionName));
                    return ResponseResult<string>.BadRequest("Error on updating permission");
                }
                
                return ResponseResult<string>
                   .Created(MessageConstants.Message(MessageOperationType.Update, "permission"));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating permission ", ex.Message);
                throw;
            }
        }

        private async Task UpdateRolePermissionGroup(List<int> roles, long permissionID)
        {
            var rolePermissionGroups = roles.Select(x => new RolePermissionGroup
            {
                PermissionID = permissionID,
                RoleID = x
            }).ToList();

            var existingRolePermission = await _unitofWork.GetRepository<RolePermissionGroup>()
                .ApplyPredicateAsync(rpg => rpg.PermissionID == permissionID);

            var rolesToRemove = await existingRolePermission
                .Where(rpg => !roles.Contains(Convert.ToInt32(rpg.RoleID)))
                .ToListAsync();

            // Determine which roles should be added (those that are not already in the existing records)
            var rolesToAdd = rolePermissionGroups
                .Where(rpg => !existingRolePermission
                    .Any(existing => existing.RoleID == rpg.RoleID))
                .ToList();

            // Remove the obsolete RolePermissionGroups
            if (rolesToRemove.Any())
            {
                await _unitofWork.GetRepository<RolePermissionGroup>().RemoveRangeAsync(rolesToRemove);
            }

            // Add the new RolePermissionGroups
            if (rolesToAdd.Any())
            {
                await _unitofWork.GetRepository<RolePermissionGroup>().AddRangeAsync(rolesToAdd);
            }
        }

        private async Task AddOrUpdatePermissionGroupsAsync(IEnumerable<PermissionGroup> newPermissionGroups)
        {
            // Fetch existing PermissionGroups that match the PermissionID in DTOs
            var existingGroups = await _unitofWork.GetRepository<PermissionGroup>()
                .ApplyPredicateAsync(e =>
                    newPermissionGroups
                        .Select(dto => dto.PermissionID)
                        .Contains(e.PermissionID));

            // Prepare entities to be added or updated
            var permissionGroupsToUpdate = existingGroups
                .AsEnumerable()
                .Join(newPermissionGroups,
                    existing => new { existing.PermissionID, existing.OperationID },
                    incoming => new { incoming.PermissionID, incoming.OperationID },
                    (existing, incoming) =>
                    {
                        existing.Access = incoming.Access;  // Update Access
                        return existing;
                    })
                .ToList();

            // Add new PermissionGroups that don't exist yet
            var permissionGroupsToAdd = newPermissionGroups
                .Where(incoming => !existingGroups
                    .Any(existing => existing.PermissionID == incoming.PermissionID &&
                                    existing.OperationID == incoming.OperationID))
                .ToList();

            // Update existing groups
            if (permissionGroupsToUpdate.Any())
            {
                await _unitofWork.GetRepository<PermissionGroup>().UpdateRangeAsync(permissionGroupsToUpdate);
            }

            // Add new groups
            if (permissionGroupsToAdd.Any())
            {
                await _unitofWork.GetRepository<PermissionGroup>().AddRangeAsync(permissionGroupsToAdd);
            }
        }

        private async Task<bool> IsPermissionExists(long currentPermissionId, string permissionName)
        {
            return await _unitofWork.GetRepository<Permission>()
                 .AnyAsync(p => p.PermissionID != currentPermissionId &&
                 p.PermissionName == permissionName);
        }

        private async Task<bool> SavePermissionAsync(CancellationToken cancellationToken)
        {
            return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
        }
    }
}