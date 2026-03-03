using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Supplier;
using Mapster;

namespace CbsAp.Application.Features.Supplier.Queries.GetSupplierLookUp
{
    public class GetSupplierLookUpQueryHandler : IQueryHandler<GetSupplierLookUpQuery, ResponseResult<IEnumerable<SupplierLookUpDto>>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetSupplierLookUpQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<IEnumerable<SupplierLookUpDto>>> Handle(
            GetSupplierLookUpQuery request,
            CancellationToken cancellationToken)
        {
            var results = await GetSupplierLookUpsAsync();

            return !results.Any() ?
               ResponseResult<IEnumerable<SupplierLookUpDto>>.NotFound("No Supplier lookup found ")
               : ResponseResult<IEnumerable<SupplierLookUpDto>>.SuccessRetrieveRecords(results);
        }

        private async Task<IEnumerable<SupplierLookUpDto>> GetSupplierLookUpsAsync()
        {
            var result = await _unitofWork.GetRepository<SupplierInfo>()
                .GetAllAsync();

            var dto = result.ProjectToType<SupplierLookUpDto>();
            return dto.AsEnumerable();
        }
    }
}