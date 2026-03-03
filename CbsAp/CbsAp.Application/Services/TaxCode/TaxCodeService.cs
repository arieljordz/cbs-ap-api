using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.TaxCode;
using CbsAp.Application.DTOs.TaxCodesManagement;
using CbsAp.Domain.Entities.TaxCodes;
using Mapster;

namespace CbsAp.Application.Services.TaxCodes
{
    public class TaxCodeService : ITaxcodeService
    {
        private readonly IUnitofWork _unitofWork;

        public TaxCodeService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<IEnumerable<TaxCodeLookupDto>> GetTaxCodeLookupAsync()
        {
            var result = await _unitofWork.GetRepository<TaxCode>()
                .GetAllAsync();

            var dto = result.ProjectToType<TaxCodeLookupDto>();
            return dto.AsEnumerable();
        }
    }
}
