using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Supplier;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Supplier.Commands.UpdateSupplier
{
    public class UpdateSupplierCommandHandler : ICommandHandler<UpdateSupplierCommand, ResponseResult<bool>>
    {
        private readonly ISupplierService _supplierService;

        private readonly ILogger<UpdateSupplierCommandHandler> _logger;

        public UpdateSupplierCommandHandler(ISupplierService supplierService, ILogger<UpdateSupplierCommandHandler> logger)
        {
            _supplierService = supplierService;

            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierService.GetSupplierInfoById(request.Supplier.SupplierInfoID);

            // workaround for preserving audit fields values for temporary resolution for mapping issue
            supplier.CreatedBy = supplier.CreatedBy!;
            supplier.CreatedDate = supplier.CreatedDate!;

            var mapUpdateSupplier = request.Supplier.Adapt(supplier);

            mapUpdateSupplier.SetAuditFieldsOnUpdate(request.UpdatedBy);

            if (await _supplierService.IsSupplierExist(request.Supplier.SupplierID!, request.Supplier.SupplierName!, request.Supplier.SupplierInfoID))
            {
                _logger.LogWarning("Supplier with name : {Name} and {SupplierName} is already existed",
                  supplier.SupplierID, supplier.SupplierName);

                return ResponseResult<bool>.BadRequest("Supplier is already existed");
            }

            if (!await _supplierService.UpdateSupplier(mapUpdateSupplier, cancellationToken))
            {
                _logger.LogError("Error in updating  supplier : {SupplierName}", supplier.SupplierName);
                return ResponseResult<bool>.BadRequest("Error on  updating  Supplier");
            }
            return ResponseResult<bool>.OK(MessageConstants.Message("supplier", MessageOperationType.Update));
        }
    }
}