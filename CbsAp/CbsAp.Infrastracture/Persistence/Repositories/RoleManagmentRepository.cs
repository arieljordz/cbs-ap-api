using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class RoleManagmentRepository : IRoleManagementRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public RoleManagmentRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<RoleManagerDTO>> GetActiveRoleManagerAsync(string name, string userId)
        {
            var predicate = PredicateBuilder.New<Role>(r => r.IsActive);

            if (!string.IsNullOrEmpty(name))
            {
                predicate = predicate.And(r => r.UserRoles.Any(ur =>
                    ur.UserAccount.FirstName.Contains(name) ||
                    ur.UserAccount.LastName.Contains(name)));
            }

            if (!string.IsNullOrEmpty(userId))
            {
                // Add a filter for user ID
                predicate = predicate.And(r => r.UserRoles.Any(ur =>
                    ur.UserAccount.UserID.Contains(userId)));
            }

            var roleManagers = await _dbcontext.Roles
                .AsNoTracking()
                .Include(r => r.UserRoles)
                .Where(predicate)
                .Select(dto => new RoleManagerDTO
                {
                    RoleID = dto.RoleID,
                    RoleName = dto.RoleName!,
                    Users = string.Join(";", dto.UserRoles
                     .Where(ur =>
                            (string.IsNullOrEmpty(name) || ur.UserAccount.FirstName.Contains(name) || ur.UserAccount.LastName.Contains(name)) &&
                            (string.IsNullOrEmpty(userId) || ur.UserAccount.UserID.Contains(userId)))
                     .Take(10)
                    .Select(ur =>
                       $"{ur.UserAccount.FirstName}{ur.UserAccount.LastName} ({ur.UserAccount.UserID})"
                    ))
                }).ToListAsync();
            return roleManagers;
        }

        public async Task<IEnumerable<RoleManagerDTO>> GetActiveRoleAsync(string? roleName, string? firstName, string? lastName)
        {
            var predicate = PredicateBuilder.New<Role>(r => r.IsActive);

            if (!string.IsNullOrEmpty(roleName))
            {
                predicate = predicate.And(r => r.RoleName!.Contains(roleName)!);
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                predicate = predicate.And(r => r.UserRoles.Any(ur =>
                    ur.UserAccount.FirstName.Contains(firstName)));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                predicate = predicate.And(r => r.UserRoles.Any(ur =>
                    ur.UserAccount.LastName.Contains(lastName)));
            }

            var roleManagers = await _dbcontext.Roles
                .AsNoTracking()
                .Include(r => r.UserRoles)
                .Where(predicate)
                .Select(dto => new RoleManagerDTO
                {
                    RoleID = dto.RoleID,
                    RoleName = dto.RoleName!,
                    Users = string.Join(",", dto.UserRoles
                    .Where(ur =>
                        (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName)) ||
                        (!string.IsNullOrEmpty(firstName) && ur.UserAccount.FirstName.Contains(firstName!)) ||
                        (!string.IsNullOrEmpty(lastName) && ur.UserAccount.LastName.Contains(lastName!))
                    )
                     .Take(5)
                    .Select(ur =>
                       $"{ur.UserAccount.FirstName} {ur.UserAccount.LastName}"
                    ))
                }).ToListAsync();
            return roleManagers;
        }

        public async Task<IEnumerable<Role>> GetAllRolesSearchAsync()
        {
            var roles = await _dbcontext.Roles

                .OrderBy(r => r.RoleID)
                .ToListAsync();

            return roles.Any() ? roles : Enumerable.Empty<Role>();
        }

        public async Task<IEnumerable<RoleDTO>> GetActiveRoleByUserAsync(string? username)
        {
            var predicate = PredicateBuilder.New<Role>(r => r.IsActive);

            if (!string.IsNullOrEmpty(username))
            {
                predicate = predicate.And(r => r.UserRoles.Any(ur =>
                  ur.UserAccount.UserID == username));
            }

            var roleManagers = await _dbcontext.Roles
                .AsNoTracking()
                .Include(r => r.UserRoles)
                .Where(predicate)
                .Select(dto => new RoleDTO
                {
                    RoleID = dto.RoleID,
                    RoleName = dto.RoleName!,
                    AuthorisationLimit = dto.AuthorisationLimit
                }).ToListAsync();
            return roleManagers;
        }

        public async Task<RoleSearchDTO?> GetUserRoleAsync(string userName, int roleId)
        {
            var predicate = PredicateBuilder.New<Role>(r => r.IsActive && r.RoleID == roleId && r.UserRoles.Any(ur =>
                  ur.UserAccount.UserID == userName));

            var role = await _dbcontext.Roles
                .AsNoTracking()
                .Include(r => r.UserRoles)
                .Where(predicate)
                .Select(dto => new RoleSearchDTO
                {
                    RoleID = dto.RoleID,
                    RoleName = dto.RoleName!,
                    AuthorisationLimit = dto.AuthorisationLimit
                }).FirstOrDefaultAsync();
            return role;
        }

        public IQueryable<Role> GetRoleAsQueryable()
        {
            return _dbcontext.Roles.AsQueryable();
        }
    }
}