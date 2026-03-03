using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Invoicing.InvActivityLog.Queries
{
    public class GetInvActivityLogQueryHandler : IQueryHandler<GetInvActivityLogQuery, ResponseResult<List<InvActivityLogDto>>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetInvActivityLogQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<List<InvActivityLogDto>>> Handle(GetInvActivityLogQuery request, CancellationToken cancellationToken)
        {
            var invActivityLogRepo = _unitofWork.GetRepository<InvoiceActivityLog>();

            var flatActionLog = await invActivityLogRepo
                .Query()
                .AsNoTracking()
                 .Where(ia => ia.InvoiceID == request.InvoiceID)
                 .OrderByDescending(ia => ia.CreatedDate)
                 .Select(h => new InvActivityLogEntriesDto
                 {
                     InvoiceID = h.InvoiceID,
                     ActivityLogID = h.ActivityLogID,
                     PreviousStatus = h.PreviousStatus.ToString(),
                     CurrentStatus = h.CurrentStatus.ToString(),
                     Reason = h.Reason,
                     Action = h.Action.ToString(),
                     CreatedBy = h.CreatedBy!,
                     CreatedDate = h.CreatedDate.HasValue ? h.CreatedDate.Value.ToAudDateTreatAsLocal().ToString("dd/MM/yyyy") : null!

                 })
                .ToListAsync(cancellationToken);

            var groupedByAction =
                flatActionLog
                .OrderByDescending(a => a.CreatedDate)
                .GroupBy(ia => ia.Action)

                .Select(a => new InvActivityLogDto
                {
                    ActionName = a.Key!,
                    InvActivityLogEntries = a.ToList(),
                }).ToList();

            return ResponseResult<List<InvActivityLogDto>>.OK(groupedByAction!);
        }
    }
}
