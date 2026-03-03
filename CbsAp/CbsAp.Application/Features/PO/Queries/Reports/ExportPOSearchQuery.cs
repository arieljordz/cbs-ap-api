using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.PO.Queries.Reports
{
    public record ExportPOSearchQuery(
        string? PONo,
        string? EntityName,
        string? Supplier,
        bool? IsActive) : IQuery<ResponseResult<byte[]>>
    {
    }
}
