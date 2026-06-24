using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.PO.Queries.ReportDetail
{
    public record ExportPODetailSearchQuery(
        long PurchaseOrderId, string? SearchLine) : IQuery<ResponseResult<byte[]>>
    {
    }
}
