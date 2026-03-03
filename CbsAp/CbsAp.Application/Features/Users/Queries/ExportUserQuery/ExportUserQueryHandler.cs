using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.UserManagement;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Users.Queries.ExportUserQuery
{
    public class ExportUserQueryHandler : IQueryHandler<ExportUserQuery, ResponseResult<byte[]>>
    {
        private readonly IExcelService _excelService;
        private readonly IUserManagementRepository _userManagementRepository;

        public ExportUserQueryHandler(IExcelService excelService, IUserManagementRepository userManagementRepository)
        {
            _excelService = excelService;
            _userManagementRepository = userManagementRepository;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportUserQuery request, CancellationToken cancellationToken)
        {
            ExpressionStarter<UserAccount> predicate
             = PredicateBuilder.New<UserAccount>(u => !u.IsUserPartialDeleted);

            if (!string.IsNullOrEmpty(request.FullName))
            {
                predicate = predicate.And(
                    SearchCustomPredicates.SearchFullName<UserAccount>(request.FullName,
                    u => u.FirstName, x => x.LastName));
            }
            if (!string.IsNullOrEmpty(request.UserId))
            {
                predicate = predicate.And(u => u.UserID.Contains(request.UserId));
            }
            if (request.IsActive.HasValue)
            {
                predicate = predicate.And(u => u.IsActive == request.IsActive);
            }

            var userQuery = await _userManagementRepository
                .GetUserAccountAsQueryable()
                .Include(u => u.UserRoles)
                .Include(u => u.UserLogInfo)
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate)
                .Select(u => new UserExportDTO
                {
                    UserID = u.UserID, 
                    FullName = $"{u.FirstName} {u.LastName}",
                    EmailAddress = u.EmailAddress ?? string.Empty,
                    IsActive = u.IsActive,
                    IsLockedOut = u.UserLogInfo.IsLockedOut,
                    LastLoginDateTime = u.UserLogInfo.LastLoginDateTime.HasValue ? u.UserLogInfo.LastLoginDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                    CountOfAssignedRoles = u.UserRoles.Count
                }).ToListAsync(cancellationToken: cancellationToken); ;

            if (userQuery.Count == 0 || userQuery == null)
            {
                return ResponseResult<byte[]>.NotFound(MessageConstants.Message("Users", MessageOperationType.NotFound));
            }

            var excelBytes = await Task.Run(() => _excelService.GenerateExcel(userQuery, "Users"));

            return ResponseResult<byte[]>.Success(excelBytes, "Export excel data");
        }
    }
}
