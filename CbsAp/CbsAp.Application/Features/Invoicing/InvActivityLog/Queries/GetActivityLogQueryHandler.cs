using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.ActivityLog;
using CbsAp.Application.DTOs.Invoicing;
using CbsAp.Domain.Entities.ActivityLog;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Supplier;

namespace CbsAp.Application.Features.Invoicing.InvActivityLog.Queries
{
    public class GetActivityLogQueryHandler : IQueryHandler<GetActivityLogQuery, ResponseResult<List<ActivityLogEntryDto>>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetActivityLogQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<List<ActivityLogEntryDto>>> Handle(GetActivityLogQuery request, CancellationToken cancellationToken)
        {
            var activityLogRepo = _unitofWork.GetRepository<ActivityLog>();


            var actionLog = await activityLogRepo
                .Query()
                .AsNoTracking()
                .Where(w => w.InvoiceID == request.InvoiceID)
                .OrderByDescending(ob => ob.ActivityDate)
                .ToListAsync(cancellationToken);

            var keywordDict = await _unitofWork.GetRepository<Keyword>()
                .Query()
                .ToDictionaryAsync(k => k.KeywordID, k => k.KeywordName, cancellationToken);

            var routingFlowDict = await _unitofWork.GetRepository<InvRoutingFlow>()
                .Query()
                .ToDictionaryAsync(r => r.InvRoutingFlowID, r => r.InvRoutingFlowName, cancellationToken);

            var supplierDict = await _unitofWork.GetRepository<SupplierInfo>()
                .Query()
                .ToDictionaryAsync(s => s.SupplierInfoID, s => s.SupplierName, cancellationToken);

            var result = actionLog.Select(s => new ActivityLogEntryDto
            {
                InvoiceID = s.InvoiceID,
                ActionDate = s.ActivityDate,
                Action = s.Activity ?? string.Empty,
                Table = s.TableName ?? string.Empty,
                Column = s.ColumnName ?? string.Empty,
                ActivityClass = s.Module ?? string.Empty,
                PrevValue = MapValue(s.OldValue, s.ColumnName, keywordDict, routingFlowDict, supplierDict),
                NewValue = s.Activity == "INSERT" ? s.metaDataNew : MapValue(s.NewValue, s.ColumnName, keywordDict, routingFlowDict, supplierDict),
                ActionedBy = s.ActionBy
            }).OrderBy(o => o.ActionDate).ToList();

            return ResponseResult<List<ActivityLogEntryDto>>.OK(result);
        }

        private string MapValue(
            string value,
            string columnName,
            Dictionary<long, string> keywordDict,
            Dictionary<long, string> routingFlowDict,
            Dictionary<long, string> supplierDict)
        {
            if (!int.TryParse(value, out var id))
                return value;

            return columnName switch
            {
                "KeywordID" => keywordDict.TryGetValue(id, out var k) ? k : value,
                "InvRoutingFlowID" => routingFlowDict.TryGetValue(id, out var r) ? r : value,
                "SupplierInfoID" => supplierDict.TryGetValue(id, out var s) ? s : value,
                "EntityProfileID" => supplierDict.TryGetValue(id, out var e) ? e : value, // verify if correct
                _ => value
            };
        }
    }
}
