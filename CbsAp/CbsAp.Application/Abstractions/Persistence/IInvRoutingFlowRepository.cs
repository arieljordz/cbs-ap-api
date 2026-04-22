using CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Invoicing;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IInvRoutingFlowRepository
    {
        Task<bool> IsInvRoutingFlowExist(long? supplierInfoID, string invRoutingFlowName, long? invRoutingFlowID = null);

        Task<InvRoutingFlow?> GetInvRoutingFlowByIdAsync(long invRoutingFlowID, CancellationToken cancellationToken);

        Task<List<ExportInvRoutingFlowDto>> ExportInvRoutingFlowToExcel(string? EntityName,
        string? InvRoutingFlowName,
        string? LinkSupplier,
        string? Roles,
        string? MatchReference, 
        CancellationToken cancellationToken);


        Task<PaginatedList<SearchInvRoutingFlowDto>> InvRoutingFlowSearchWithPagination(string? EntityName,
           string? InvRoutingFlowName,
           string? LinkSupplier,
           string? Roles,
           string? MatchReference,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
           CancellationToken cancellationToken);

        Task<PaginatedList<SearchInvRoutingFlowRolesDto>> InvRoutingFlowSearchWithRolesPagination(
            long? InvRoutingFlowID,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
           CancellationToken cancellationToken);
        Task<PaginatedList<SearchInvRoutingFlowUserDto>> InvRoutingFlowSearchWithUsersPagination(
         long? InvRoutingFlowID,
         int pageNumber,
         int pageSize,
         string? sortField,
         int? sortOrder,
        CancellationToken cancellationToken);


        Task<List<InvInfoRoutingLevelDto>> GetInvInfoRoutingFlow(
            long invoiceID,
            long? keywordID,
            long? supplierInfoID,
            CancellationToken cancellationToken);
    }
}