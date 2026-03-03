using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;

namespace CbsAp.Application.Abstractions.Services.Invoicing
{
    public interface IInvRoutingFlowService
    {
        Task<IEnumerable<InvRoutingFlowLookupDto>> GetInvRoutingFlowLookupsAsync();
    }
}
