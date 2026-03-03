using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.PO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.PO.Queries.GetPOLineUsage
{
    public class GetPOLineUsageQueryHandler : IQueryHandler<GetPOLineUsageQuery, ResponseResult<decimal>>
    {
        private readonly IUnitofWork _unitofWork;
        public GetPOLineUsageQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<decimal>> Handle(GetPOLineUsageQuery request, CancellationToken cancellationToken)
        {
            var pomatchingRepo = _unitofWork.GetRepository<PurchaseOrderMatchTracking>();

           
            
            var totalMatchedQty = await pomatchingRepo
                .Query()
                .Where(x=> x.PurchaseOrderLineID == request.PurchaseOrderLineID)
                .SumAsync(s => (decimal?)s.Qty, cancellationToken) ?? 0m;

            return ResponseResult<decimal>.OK(totalMatchedQty);
        }
    }
}
