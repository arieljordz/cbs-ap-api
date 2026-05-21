using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Dashboard;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using LinqKit;

using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Linq.Expressions;

using System.Text.RegularExpressions;
using CbsAp.Domain.Entities.UserManagement;

namespace CbsAp.Application.Features.Dashboard.Queries
{
    public class GetAssignedInvoiceQueryHandler : IQueryHandler<GetAssignedInvoiceQuery, ResponseResult<AssignedInvoiceResultDTO>>
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IPermissionManagementRepository _permissionManagementRepository;

        public GetAssignedInvoiceQueryHandler(IUnitofWork unitOfWork, IPermissionManagementRepository permissionManagementRepository)
        {
            _unitOfWork = unitOfWork;
            _permissionManagementRepository = permissionManagementRepository;
        }

        public async Task<ResponseResult<AssignedInvoiceResultDTO>> Handle(GetAssignedInvoiceQuery request, CancellationToken cancellationToken)
        {

            var permissionOperations = _permissionManagementRepository.GetOperationsByRole(request.RoleId);

            var userRoles = await _unitOfWork
                .GetRepository<UserAccount>()
                .Query()
                .Where(u => u.UserID == request.CurrentUser)
                .SelectMany(u => u.UserRoles.Select(r => r.RoleID))
                .ToListAsync(cancellationToken);


            var repository = _unitOfWork.GetRepository<Invoice>();

            // Build permission predicate (without due-date filter)
            ExpressionStarter<Invoice> permissionPredicate = PredicateBuilder.New<Invoice>();
            bool hasPermission = false;

            if (permissionOperations.Any(x => x.OperationName == "My Invoices"))
            {
                permissionPredicate = permissionPredicate.Or(i => (i.StatusType == InvoiceStatusType.ForApproval
                                                                 || i.StatusType == InvoiceStatusType.ApprovalOnHold
                                                                 || i.QueueType == InvoiceQueueType.MyInvoices)
                                                                 && i.ApproverRole != null
                                                                 && userRoles.Contains((long)i.ApproverRole));
                hasPermission = true;
            }
            
            ExpressionStarter<Invoice> filteredPredicate = PredicateBuilder.New<Invoice>(permissionPredicate);
            if (hasPermission && string.Equals(request.FilterType, "overdue", StringComparison.OrdinalIgnoreCase))
            {
                filteredPredicate = filteredPredicate.And(i => i.DueDate != null && i.DueDate < DateTime.UtcNow);
            }

            // Queries
            var baseQuery = repository.Query().Where(permissionPredicate);

            // Build filteredQuery and order by the latest InvoiceActionLog.CreatedDate where Action == 12 (top 1 by CreatedDate desc)
            var filteredQuery = repository.Query()
                .Where(filteredPredicate)
                .Select(i => new
                {
                    Invoice = i,
                    LatestActionCreatedDate = i.InvoiceActivityLog!
                        .Where(l => l.Action == InvoiceActionType.Import || l.Action == InvoiceActionType.RouteToException || l.Action == InvoiceActionType.Submit)
                        .OrderByDescending(l => l.CreatedDate)
                        .Select(l => l.CreatedDate)
                        .FirstOrDefault()
                })
                .OrderByDescending(x => x.LatestActionCreatedDate)
                .Select(x => x.Invoice);

            // Counts
            var totalCount = await baseQuery.CountAsync(cancellationToken);
            var overdueCount = await baseQuery.CountAsync(i => i.DueDate != null && i.DueDate < DateTime.UtcNow, cancellationToken);

            // Fetch list using filteredQuery (respects overdue filter and is ordered by LatestActionCreatedDate)            
            var list = await filteredQuery.Select(i => new AssignedInvoiceDTO
            {
                InvoiceId = i.InvoiceID,
                Queue = Regex.Replace(i.QueueType!.ToString()!, "([a-z])([A-Z])", "$1 $2"),
                SupplierName = i.SupplierInfo == null ? "" : i.SupplierInfo.SupplierName,
                InvoiceDate = i.InvoiceDate == null ? null : i.InvoiceDate!.Value.UtcDateTime,
                InvoiceNumber = i.InvoiceNo,
                Amount = i.TotalAmount,
                DueDate = i.DueDate == null ? null : i.DueDate!.Value.UtcDateTime,
                AssignedRole = i.ApproverInvoices != null ? i.ApproverInvoices.RoleName : string.Empty,
                AssignedRoleId = i.ApproverRole
            }).ToListAsync(cancellationToken);

            var resultDto = new AssignedInvoiceResultDTO
            {
                Invoices = list,
                OverdueCount = overdueCount,
                TotalCount = totalCount
            };

            return ResponseResult<AssignedInvoiceResultDTO>.OK(resultDto);
        }
    }
}