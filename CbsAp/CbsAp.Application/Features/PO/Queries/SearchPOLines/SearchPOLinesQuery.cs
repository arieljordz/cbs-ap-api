using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PO.Queries.SearchPOLines
{
    public record SearchPOLinesQuery(
        string? SupplierName,
        string? SupplierTaxID,
        string? PONo,
        DateTime? PODateFrom,
        DateTime? PODateTo,
        string? DeliveryNo,
        string? SupplierNo,
        List<long>? ExcludesMatchPOLineIds,
        bool IsAvailableOrder 
        // bool? IsAvailableOrder  TODO
        ) :
        IQuery<ResponseResult<List<SearchPoLinesDto>>>
    {
    }
}