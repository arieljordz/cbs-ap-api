using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.PermissionManagement.Commands.CreatePermission
{
    public class CreatePermissionHandler : ICommandHandler<PermissionCommand, ResponseResult<string>>
    {
        private readonly ILogger<CreatePermissionHandler> _logger;
        private readonly IUnitofWork _unitofWork;

        public CreatePermissionHandler(IUnitofWork unitofWork,
            ILogger<CreatePermissionHandler> logger
            )
        {
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public async Task<ResponseResult<string>> Handle(PermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var permissionDTO = request.CreatePermissionDTO.Adapt<PermissionDTO>();
                var permission = permissionDTO.Adapt<Permission>();
                permission.SetAuditFieldsOnCreate(request.CreatedBy!);

                //Validate if permission name is exist
                if (await IsPermissionExists(permission))
                {
                    return ResponseResult<string>.Confict("Permission is already exist");
                }

                var permissionGroups = request.CreatePermissionDTO.GroupPanel
                  .Where(gp => gp.Operations.Any(o => o.Access))
                  .SelectMany(gp => gp.Operations)
                   .Select(op => new PermissionGroup
                   {
                       OperationID = op.OperationID,
                       Access = op.Access,
                   }).ToList();

                var rolePermissionGroup =
                   permissionDTO.Roles.Select(rpg => new RolePermissionGroup
                   {
                       RoleID = rpg
                   }).ToList();

                permission.PermissionGroups = permissionGroups;
                permission.RolePermissionGroups = rolePermissionGroup;
                permission.IsActive = true;
                await _unitofWork.GetRepository<Permission>().AddAsync(permission);
                await _unitofWork.SaveChanges(cancellationToken);

                if (!await SavePermissionAsync(cancellationToken))
                {
                    _logger.LogError(MessageConstants.FormatMessage(MessageConstants.AddError, permission.PermissionName));
                    return ResponseResult<string>.BadRequest("Error on creating new permission");
                }
                return ResponseResult<string>
                   .Created(MessageConstants.Message(MessageOperationType.Create, "permission"));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating a new permission.", ex.Message);
                return ResponseResult<string>.InternalServerError(ex.Message);
            }
        }

        private async Task AddNewPermissionAsync(Permission permission)
        {
            await _unitofWork.GetRepository<Permission>().AddAsync(permission);
        }

        private async Task AddPermissionGroupAsync(PermissionCommand request, Permission permission, CancellationToken cancellationToken)
        {
            //   //var permissionGroupDto = request.CreatePermissionDTO.GroupPanel
            //   //    .SelectMany(gp => gp.Operations)
            //   //    .Select(op => new PermissionGroupDTO
            //   //    {
            //   //        PermissionID = permission.PermissionID,
            //   //        OperationID = op.OperationID,
            //   //        Access = op.Access,
            //   //    }).ToList();

            //   //IEnumerable<PermissionGroup>? permissionGroup =
            //   //    permissionGroupDto.Adapt<IEnumerable<PermissionGroup>>();

            //   //await _unitofWork.GetRepository<PermissionGroup>().AddRangeAsync(permissionGroup);
            //   //await _unitofWork.SaveChanges(cancellationToken);
            //   //var permissionGroup = request.CreatePermissionDTO.GroupPanel
            //   //    .SelectMany(gp => gp.Operations)
            //   //    .Select(op => new PermissionGroup
            //   //    {
            //   //        PermissionID = permission.PermissionID,
            //   //        OperationID =
            //   //    });
            //   //await _unitofWork.GetRepository<PermissionGroup>().AddRangeAsync(permissionGroup);
            //   //todo
            //   var permissionGroupDto = request.CreatePermissionDTO.GroupPanel
            //.SelectMany(gp => gp.Operations)
            //.Select(op => new PermissionGroupDTO
            //{
            //    PermissionID = permission.PermissionID,
            //    OperationID = op.OperationID,
            //    Access = op.Access,
            //});

            //   IEnumerable<PermissionGroup> permissionGroups = permissionGroupDto.Adapt<IEnumerable<PermissionGroup>>();

            //   // Get repository
            //   var permissionGroupRepo = _unitofWork.GetRepository<PermissionGroup>();

            //   // Fetch existing entities from the database
            //   //var existingGroups = await permissionGroupRepo.GetAllAsync()
            //   //    .Result().wher(pg => permissionGroups.Any(g =>
            //   //        g.PermissionID == pg.PermissionID &&
            //   //        g.OperationID == pg.OperationID))
            //   //    .ToListAsync(cancellationToken);

            //   var existingGroups = await permissionGroupRepo.GetAllAsync();

            //   existingGroups.Where(pg => permissionGroups.Any(g =>
            //           g.PermissionID == pg.PermissionID &&
            //           g.OperationID == pg.OperationID))
            //       .ToListAsync(cancellationToken);

            //   // Remove existing entities from the collection
            //   var newGroups = permissionGroups
            //       .Where(pg => !existingGroups.Any(eg =>
            //           eg.PermissionID == pg.PermissionID &&
            //           eg.OperationID == pg.OperationID))
            //       .ToList();

            //   // Detach any conflicting tracked entities
            //   foreach (var group in newGroups)
            //   {
            //       var trackedEntity = _unitofWork.Context.ChangeTracker.Entries<PermissionGroup>()
            //           .FirstOrDefault(e =>
            //               e.Entity.PermissionID == group.PermissionID &&
            //               e.Entity.OperationID == group.OperationID);

            //       bool trcacked = _unitofWork.Context.Entry(trackedEntity.Entity).State != EntityState.Detached;

            //       if (trackedEntity != null)
            //       {
            //           _unitofWork.Context.Entry(trackedEntity.Entity).State = EntityState.Detached;
            //       }
            //   }

            //   // Add only new entities
            //   if (newGroups.Any())
            //   {
            //       //_unitofWork.Context.ChangeTracker
            //       await permissionGroupRepo.AddRangeAsync(newGroups);
            //   }

            //   // Save changes
            //   await _unitofWork.SaveChanges(cancellationToken);
        }

        private async Task AddPermissionRolesAsync(PermissionDTO permissionDTO, Permission permission)
        {
            if (permissionDTO.Roles.Any())
            {
                var rolePermissionGroupDTO =
                    permissionDTO.Roles.Select(rpg => new RolePermissionGroup
                    {
                        PermissionID = permission.PermissionID,
                        RoleID = rpg
                    });

                var rolePermission =
                    rolePermissionGroupDTO.Adapt<IEnumerable<RolePermissionGroup>>();

                await _unitofWork.GetRepository<RolePermissionGroup>().AddRangeAsync(rolePermission);
            }
        }

        private async Task<bool> IsPermissionExists(Permission permission)
        {
            return await _unitofWork.GetRepository<Permission>()
                 .AnyAsync(p => p.PermissionName == permission.PermissionName);
        }

        private async Task<bool> SavePermissionAsync(CancellationToken cancellationToken)
        {
            return await _unitofWork.SaveChanges(cancellationToken);
        }
    }
}