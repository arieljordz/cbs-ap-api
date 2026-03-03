using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Invoicing;
using CbsAp.Application.DTOs.Invoicing.Accounts;
using CbsAp.Domain.Entities.Invoicing;
using Mapster;

namespace CbsAp.Application.Services.Invoicing
{
    public class AccountsService : IAccountsService
    {
        private readonly IUnitofWork _unitofWork;

        public AccountsService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public async Task<IEnumerable<AccountLookupDto>> GetAccountLookupsAsync()
        {
            var result = await _unitofWork.GetRepository<Account>()
               .GetAllAsync();

            var dto = result.ProjectToType<AccountLookupDto>();
            return dto.AsEnumerable();

        }
    }
}
