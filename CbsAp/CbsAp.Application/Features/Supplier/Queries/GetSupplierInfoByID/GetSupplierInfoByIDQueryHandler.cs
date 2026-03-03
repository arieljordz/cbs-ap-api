using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Supplier;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared.ResultPatten;
using Mapster;

namespace CbsAp.Application.Features.Supplier.Queries.GetSupplierInfoByID
{
    public class GetSupplierInfoByIDQueryHandler : IQueryHandler<GetSupplierInfoByIDQuery, ResponseResult<SupplierInfoDto>>
    {
        private readonly ISupplierService _supplierService;

        public GetSupplierInfoByIDQueryHandler(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<ResponseResult<SupplierInfoDto>> Handle(GetSupplierInfoByIDQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierService.GetSupplierInfoById(request.supplierInfoID)!;

            var dto = supplier.Adapt<SupplierInfoDto>();
            return dto == null ?
                ResponseResult<SupplierInfoDto>.BadRequest("Supplier  not found") :
                ResponseResult<SupplierInfoDto>.OK(dto);
        }
    }
}