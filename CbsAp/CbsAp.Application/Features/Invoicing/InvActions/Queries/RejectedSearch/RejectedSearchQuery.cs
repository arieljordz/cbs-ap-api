using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.RejectedSearch
{
    public class RejectedSearchQuery : IQuery<ResponseResult<PaginatedList<RejectedInvoiceSearchDto>>>
    {
        public string? SupplierName { get; set; }
        public string? InvoiceNo { get; set; }
        public string? PONo { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string? sortField { get; set; }
        public int? sortOrder { get; set; }
        public int RoleId { get; set; }
    }
}