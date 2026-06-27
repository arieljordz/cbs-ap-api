using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Features.Roles.Queries.Common;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.DimensionSetup;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Roles.Queries.SearchHandler
{
    public class GetRoleByIdQueryHandler : IQueryHandler<SearchRoleByIdQuery, ResponseResult<SearchRoleDtO>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IRoleManagementRepository _roleManagementRepository;
        private readonly IDimensionRepository _dimensionRepository;
        private readonly IDimensionSetupRepository _dimensionSetupRepository;
        private readonly IAccountRepository _accountRepository;

        public GetRoleByIdQueryHandler(IUnitofWork unitofWork,IRoleManagementRepository roleManagementRepository,
            IDimensionSetupRepository dimensionSetupRepository,IDimensionRepository dimensionRepository, 
            IAccountRepository accountRepository)
        {
            _unitofWork = unitofWork;
            _roleManagementRepository = roleManagementRepository;
            _dimensionRepository = dimensionRepository;
            _dimensionSetupRepository = dimensionSetupRepository;
            _accountRepository = accountRepository;
        }
        
        public async Task<ResponseResult<SearchRoleDtO>> Handle(
            SearchRoleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var role = await _roleManagementRepository.GetRoleAsQueryable()
                .Include(rn => rn.RoleReminderNotification)
                .Include(re => re.RoleEntities)
                    .ThenInclude(re => re.EntityProfile)
                .Include(ru => ru.UserRoles)
                    .ThenInclude(ur => ur.UserAccount)
                .Include(rm1 => rm1.RelatedRoleManager1)
                .Include(rm2 => rm2.RelatedRoleManager2)
                .Include(rp => rp.RolePermissionGroups)
                    .ThenInclude(rpg => rpg.Permission)
                .FirstOrDefaultAsync(r => r.RoleID == request.roleID, cancellationToken);

            if (role == null)
                return ResponseResult<SearchRoleDtO>.NotFound("Role not Found");

            var selectedEntityProfileIds = role.RoleEntities
                .Select(x => x.EntityProfileID)
                .Distinct()
                .ToList();

            var allDimensions = await _dimensionRepository
                .GetDimensionsAsQueryable()
                .Include(d => d.EntityProfile)
                .ToListAsync(cancellationToken);

            var entityOptions = role.RoleEntities
                .Where(x => selectedEntityProfileIds.Contains(x.EntityProfileID))
                .Select(x => new DropdownOptionDto
                {
                    Value = x.EntityProfileID.ToString(),
                    Label = x.EntityProfile?.EntityName ?? string.Empty
                })
                .DistinctBy(x => x.Value)
                .OrderBy(x => x.Label)
                .ToList();

            var accounts = await _accountRepository
                .GetAccountsAsQueryable()
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync(cancellationToken);

            var dimensionSetups = await _dimensionSetupRepository
                .GetDimensionSetupAsQueryable()
                .AsNoTracking()
                .Where(x => x.Show == true)
                .ToListAsync(cancellationToken);

            var categoryOptions = dimensionSetups
                .Select(x => new DropdownOptionDto
                {
                    Value = x.DimensionSetupId.ToString(),
                    Label = x.DimensionSetupName ?? string.Empty
                })
                .Concat(accounts.Select(x => new DropdownOptionDto
                {
                    Value = x.AccountID.ToString(),
                    Label = x.AccountName ?? string.Empty
                }))
                .OrderBy(x => x.Label)
                .ToList();

            var searchRoleDto = new SearchRoleDtO
            {
                RoleID = role.RoleID,
                RoleName = role.RoleName,
                RoleManager1 = role.RoleManager1,
                RoleManager2 = role.RoleManager2,
                RoleManager1Name = role.RelatedRoleManager1 != null ? role.RelatedRoleManager1.RoleName : string.Empty,
                RoleManager2Name = role.RelatedRoleManager2 != null ? role.RelatedRoleManager2.RoleName : string.Empty,
                AuthorisationLimit = role.AuthorisationLimit,
                IsActive = role.IsActive,
                CanBeAddedToInvoice = role.CanBeAddedToInvoice,
                ReminderNotification = role.RoleReminderNotification != null ? new RoleReminderNotificationDto
                {
                    IsNewInvoiceReceiveNotification = role.RoleReminderNotification.IsNewInvoiceReceiveNotification,
                    InvoiceDueDateNotification = role.RoleReminderNotification.InvoiceDueDateNotification,
                    InvoiceEscalateToLevel1ManagerNotification = role.RoleReminderNotification.InvoiceEscalateToLevel1ManagerNotification,
                    ForwardToLevel1Manager = role.RoleReminderNotification.ForwardToLevel1Manager,
                    ForwardToLevel2Manager = role.RoleReminderNotification.ForwardToLevel2Manager
                } : null,
                RoleEntities = [.. (role.RoleEntities?
                    .Select(re => new RoleEntitiyDto
                    {
                        EntityProfileID = re.EntityProfileID,
                        EntityName = re.EntityProfile?.EntityName ?? string.Empty,
                        EntityCode = re.EntityProfile?.EntityCode ?? string.Empty
                    }) ?? Enumerable.Empty<RoleEntitiyDto>())],
                RolePermissions = [.. role.RolePermissionGroups.Select(rp => new RolePermissionDto
                    {
                        PermissionID = rp.PermissionID,
                        PermissionName = rp.Permission.PermissionName
                    }) ?? Enumerable.Empty<RolePermissionDto>()],
                RoleUsers = [.. role.UserRoles.Select(ru => new RoleUserDto
                    {
                        UserAccountID = ru.UserAccountID,
                        UserID = ru.UserAccount.UserID,
                        FullName = ru.UserAccount.FirstName + " " + ru.UserAccount.LastName
                    }) ?? Enumerable.Empty<RoleUserDto>()],

                RoleDimensions = [.. allDimensions
                    .Where(x => selectedEntityProfileIds.Contains(x.EntityProfileID))
                    .Select(d => new RoleDimensionDto
                    {
                        EntityProfileID = d.EntityProfileID,
                        DimensionID = d.DimensionID,
                        Assigned = $"{d.DimensionCode}-{d.Name}",
                    })],

                EntityOptions = entityOptions,

                CategoryOptions = categoryOptions
            };

            return ResponseResult<SearchRoleDtO>.SuccessRetrieveRecords(searchRoleDto, "Role");
        }
    }
}