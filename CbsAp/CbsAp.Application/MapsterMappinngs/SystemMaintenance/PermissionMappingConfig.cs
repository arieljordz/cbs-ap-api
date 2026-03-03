using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.DTOs.PermissionManagement.OperationDTO;
using CbsAp.Application.Features.PermissionManagement.Commands.CreatePermission;
using CbsAp.Application.Features.PermissionManagement.Commands.UpdatePermission;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.PermissionManagement;
using Mapster;

namespace CbsAp.Application.MapsterMappinngs.SystemMaintenance
{
    public class PermissionMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PermissionCommand, PermissionDTO>()
                 .Map(dest => dest.PermissionName, src => src.CreatePermissionDTO.PermissionName)
                 .Map(dest => dest.Roles, src => src.CreatePermissionDTO.Roles);
            // Update Permission
            config.NewConfig<UpdatePermissionCommand, UpdatePermissionDTO>()
                 .Map(dest => dest.PermissionID, src => src.updatePermissionDTO.PermissionID)
                 .Map(dest => dest.PermissionName, src => src.updatePermissionDTO.PermissionName)
                 .Map(dest => dest.GroupPanel, src => src.updatePermissionDTO.GroupPanel)
                 .Map(dest => dest.Roles, src => src.updatePermissionDTO.Roles);

            // Operations
            config.NewConfig<Operation, OperationsDTO>()
                .Map(dest => dest.OperationID, src => src.OperationID)
                .Map(dest => dest.OperationName, src => src.OperationName)
                .Map(dest => dest.ApplyOperationIn, src => src.ApplyOperationIn)
                .Map(dest => dest.Panel, src => src.Panel);

            //Permission Groups
            config.NewConfig<PermissionGroupDTO, PermissionGroup>()
                .MapWith(dto => new PermissionGroup
                {
                    PermissionID = dto.PermissionID,
                    OperationID = dto.OperationID,
                    Access = dto.Access,
                });

            // Roles Permission Group
            config.NewConfig<RolePermissionGroupDTO, RolePermissionGroup>()
              .MapWith(dto => new RolePermissionGroup
              {
                  PermissionID = dto.PermissionID,
                  RoleID = dto.RoleID,
              });


            config.NewConfig<Permission, PermissionSearchDTO>()
               .MapWith(permission => new PermissionSearchDTO
               {
                   IsActive = permission.IsActive
               });

            //Search Permission Group
            config.NewConfig<PaginatedList<Permission>, PaginatedList<PermissionSearchDTO>>()
                .MapWith(permission => new PaginatedList<PermissionSearchDTO>
                {
                    Data = MapPermissionSearchDTO(permission).Data,
                    CurrentPage = permission.CurrentPage,
                    PageSize = permission.PageSize,
                    TotalCount = permission.TotalCount,
                    TotalPages = permission.TotalPages,
                    IsSuccess = permission.IsSuccess,
                });

            
        }

        private static PaginatedList<PermissionSearchDTO> MapPermissionSearchDTO(PaginatedList<Permission> permission)
        {
            // Map each UserAccount object to UserAccountDTO
            var permissionDTO = permission.Data.Adapt<List<PermissionSearchDTO>>();

            // Return a new PaginatedList<UserAccountDTO> with the mapped data and other properties preserved
            return new PaginatedList<PermissionSearchDTO>(permissionDTO);
        }
    }
}