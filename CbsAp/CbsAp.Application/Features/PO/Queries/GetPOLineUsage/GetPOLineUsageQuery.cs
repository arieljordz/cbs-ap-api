using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.PO.Queries.GetPOLineUsage
{
    public record GetPOLineUsageQuery(long PurchaseOrderLineID) :
        IQuery<ResponseResult<decimal>>
    {
    }
}
