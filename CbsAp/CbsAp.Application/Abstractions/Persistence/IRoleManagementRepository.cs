using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Entities.TaxCodes;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IRoleManagementRepository
    {
        Task<IEnumerable<Role>> GetAllRolesSearchAsync();

        Task<IEnumerable<RoleManagerDTO>> GetActiveRoleManagerAsync(string name, string userId);

        Task<IEnumerable<RoleManagerDTO>> GetActiveRoleAsync(string? roleName, string? firstName, string?lastName);

        Task<IEnumerable<RoleDTO>> GetActiveRoleByUserAsync(string? username);

        IQueryable<Role> GetRoleAsQueryable();
        Task<RoleSearchDTO?> GetUserRoleAsync(string userName, int roleId);

    }
}