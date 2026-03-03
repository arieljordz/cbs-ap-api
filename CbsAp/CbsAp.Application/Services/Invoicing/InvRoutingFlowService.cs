using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Invoicing;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Domain.Entities.Invoicing;
using Mapster;

namespace CbsAp.Application.Services.Invoicing
{
    public class InvRoutingFlowService : IInvRoutingFlowService
    {
        private readonly IUnitofWork _unitofWork;

        public InvRoutingFlowService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<IEnumerable<InvRoutingFlowLookupDto>> GetInvRoutingFlowLookupsAsync()
        {
            var result = await _unitofWork.GetRepository<InvRoutingFlow>()
              .GetAllAsync();

            var dto = result.ProjectToType<InvRoutingFlowLookupDto>();
            return dto.AsEnumerable();
        }
    }
}
