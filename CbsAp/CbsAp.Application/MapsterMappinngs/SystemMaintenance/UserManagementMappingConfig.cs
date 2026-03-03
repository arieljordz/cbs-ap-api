using CbsAp.Application.DTOs;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Entities.UserManagement;
using Mapster;

namespace CbsAp.Application.MapsterMappinngs.SystemMaintenance
{
    public class UserManagementMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<bool, bool>()
               .Map(dest => dest, src => src);

            config.NewConfig<UpdatePasswordDTO, UserLogInfo>()
                .Map(dest => dest.UserID, src => src.UserID)
                .Map(dest => dest.UserLogInfoID, src => src.UserLogInfoID)
                .Map(dest => dest.UserAccountID, src => src.UserAccountID)
                .Map(dest => dest.PasswordSalt, src => src.PasswordSalt)
                .Map(dest => dest.PasswordHash, src => src.PasswordHash)
                .Map(dest => dest.LastUpdatedBy, src => src.LastUpdatedBy);

            config.NewConfig<CreateUserDTO, UserAccount>()
                .MapWith(dto => new UserAccount
                {
                    UserID = dto.UserID,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    EmailAddress = dto.EmailAddress,
                    IsActive = dto.IsActive,

                });

            config.NewConfig<UpdateUserDTO, UserAccount>()
               .MapWith(dto => new UserAccount
               {
                   UserAccountID = dto.UserAccountID,
                   UserID = dto.UserID,
                   FirstName = dto.FirstName,
                   LastName = dto.LastName,
                   EmailAddress = dto.EmailAddress,
                   IsActive = dto.IsActive,
                   //  LastUpdatedBy = dto.UpdatedBy,
               });

            config.NewConfig<UserAccount, UserSearchPaginationDTO>()
                .MapWith(userAccount => new UserSearchPaginationDTO
                {
                    UserAccountID = userAccount.UserAccountID,
                    UserID = userAccount.UserID,
                    FirstName = userAccount.FirstName,
                    LastName = userAccount.LastName,
                    EmailAddress = userAccount.EmailAddress,
                    IsActive = userAccount.IsActive,
                    FullName = $"{userAccount.FirstName} {userAccount.LastName}",
                    UserRoles = MapUserRolesToRoleDTO(userAccount.UserRoles.ToList()),
                    CountOfAssignedRoles = userAccount.UserRoles.Count,
                    LastLoginDateTime = userAccount.UserLogInfo.LastLoginDateTime != null && userAccount.UserLogInfo.LastLoginDateTime.HasValue
                            ? userAccount.UserLogInfo.LastLoginDateTime.Value.ToLocalTime().ToString("dd/MM/yyyy hh:mm:ss tt")
                            : "",
                    IsLockedOut = userAccount.UserLogInfo.IsLockedOut
                });

            config.NewConfig<PaginatedList<UserAccount>, PaginatedList<UserSearchPaginationDTO>>()
                .MapWith(userAccount => new PaginatedList<UserSearchPaginationDTO>
                {
                    Data = MapUserSearchWithRolesDTO(userAccount).Data,
                    CurrentPage = userAccount.CurrentPage,
                    PageSize = userAccount.PageSize,
                    TotalCount = userAccount.TotalCount,
                    TotalPages = userAccount.TotalPages,
                    IsSuccess = userAccount.IsSuccess,
                });

            config.NewConfig<UserAccount, ActiveUsersDTO>()
                .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
        }

        private static List<RoleDTO> MapUserRolesToRoleDTO(List<UserRole> userRoles)
        {
            return userRoles.Select(usr => usr.Role).Adapt<List<RoleDTO>>();
        }

        private static PaginatedList<UserSearchPaginationDTO> MapUserSearchWithRolesDTO(PaginatedList<UserAccount> users)
        {
            // Map each UserAccount object to UserAccountDTO
            var userAccountDTOs = users.Data.Adapt<List<UserSearchPaginationDTO>>();

            // Return a new PaginatedList<UserAccountDTO> with the mapped data and other properties preserved
            return new PaginatedList<UserSearchPaginationDTO>(userAccountDTOs);
        }
    }
}
