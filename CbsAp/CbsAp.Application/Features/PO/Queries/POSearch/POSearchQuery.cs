using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.PO.Queries.POSearch
{
    public record class POSearchQuery(
    string? PONo,
    string? EntityName,
    string? Supplier,
    bool? IsActive,
    int pageNumber,
    int pageSize,
    string? sortField,
    int? sortOrder,
    CancellationToken token) : IQuery<ResponseResult<PaginatedList<POSearchDto>>>
    {
    }
}