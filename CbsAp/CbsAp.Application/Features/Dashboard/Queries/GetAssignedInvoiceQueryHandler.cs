using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Dashboard;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Entities.UserManagement;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;


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
            var repository = _unitOfWork.GetRepository<Invoice>();
            //Get role permissions
            var permissionOperations = _permissionManagementRepository
                 .GetOperationsByRole(request.RoleId);
            //Get entities assigned to current role
            var roleEntityIds = await _unitOfWork
                .GetRepository<RoleEntity>()
                .Query()
                .Where(r => r.RoleID == request.RoleId)
                .Select(r => r.EntityProfileID)
                .ToListAsync(cancellationToken);

            ExpressionStarter<Invoice> predicate =
                PredicateBuilder.New<Invoice>(true);



            // Build permission predicate (without due-date filter)
            //   ExpressionStarter<Invoice> permissionPredicate = PredicateBuilder.New<Invoice>();
            //   bool hasPermission = false;

            // Apply entity filter Only when role has assigned entities

            if (roleEntityIds.Any())

            {
                predicate = predicate.And(i =>
                    i.EntityProfileID.HasValue &&
                    roleEntityIds.Contains(i.EntityProfileID.Value));
            }

            if (permissionOperations.Any(x => x.OperationName == "My Invoices"))
            {
                predicate = predicate.And(i =>
                i.StatusType == InvoiceStatusType.ForApproval ||
                i.StatusType == InvoiceStatusType.ApprovalOnHold ||
                i.QueueType == InvoiceQueueType.MyInvoices);
                //&& i.ApproverRole ! = null
                //&& userRoles.Contains((long)i.ApproverRole));

            }

            //overdue filter

            if (string.Equals(

                request.FilterType,
                "overdue",
                StringComparison.OrdinalIgnoreCase))
            {
                predicate = predicate.And(i =>

                i.DueDate != null &&
                i.DueDate < DateTime.UtcNow);


            }


            // Queries
            var baseQuery = repository
                .Query()
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            var totalCount = await baseQuery.CountAsync(cancellationToken);


            var overdueCount = await baseQuery.CountAsync(

                i => i.DueDate != null &&
                     i.DueDate < DateTime.UtcNow,
                cancellationToken);

            var query = baseQuery


              .Select(i => new

              {
                  Invoice = i,

                  LatestActionDate = i.InvoiceActivityLog!

              .Where(i =>

                    i.Action == InvoiceActionType.Import ||
                    i.Action == InvoiceActionType.RouteToException ||
                    i.Action == InvoiceActionType.Submit)

                     .OrderByDescending(i => i.CreatedDate)
                      .Select(i => i.CreatedDate)
                      .FirstOrDefault()
              })
              .OrderByDescending(x => x.LatestActionDate)
              .Select(x => x.Invoice);

            var invoices = await query
                 .Select(i => new AssignedInvoiceDTO
                 {
                     InvoiceId = i.InvoiceID,
                     Queue = Regex.Replace(
                         i.QueueType!.ToString(),
                         "([a-z])([A-Z])",
                         "$1 $2"),
                     SupplierName = i.SupplierInfo == null
                         ? string.Empty
                         : i.SupplierInfo.SupplierName,
                     InvoiceDate = i.InvoiceDate == null
                         ? null
                         : i.InvoiceDate.Value.UtcDateTime,
                     InvoiceNumber = i.InvoiceNo,
                     Amount = i.TotalAmount,
                     DueDate = i.DueDate == null
                         ? null
                         : i.DueDate.Value.UtcDateTime,
                     AssignedRole = i.ApproverInvoices != null
                         ? i.ApproverInvoices.RoleName
                         : string.Empty,
                     AssignedRoleId = i.ApproverRole
                 })
                 .ToListAsync(cancellationToken);


            return ResponseResult<AssignedInvoiceResultDTO>.OK(
                new AssignedInvoiceResultDTO
                {
                    Invoices = invoices,
                    OverdueCount = overdueCount,
                    TotalCount = totalCount
                });
        }
    }

}