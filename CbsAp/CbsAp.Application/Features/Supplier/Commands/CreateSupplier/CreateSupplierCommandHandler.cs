using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Supplier;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Supplier.Commands.CreateSupplier
{
    public class CreateSupplierCommandHandler : ICommandHandler<CreateSupplierCommand, ResponseResult<bool>>
    {
        private readonly ISupplierService _supplierService;

        private readonly ILogger<CreateSupplierCommandHandler> _logger;

        public CreateSupplierCommandHandler(ISupplierService supplierService, ILogger<CreateSupplierCommandHandler> logger)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = request.Supplier.Adapt<SupplierInfo>();
            supplier.SetAuditFieldsOnCreate(request.CreatedBy);
            if (await _supplierService.IsSupplierExist(supplier.SupplierID!, supplier.SupplierName!))
            {
                _logger.LogWarning("Supplier with name : {Name} and {SupplierName} is already existed",
                  supplier.SupplierID, supplier.SupplierName);

                return ResponseResult<bool>.BadRequest("Supplier is already existed");
            }

            if (!await _supplierService.CreateSupplier(supplier, cancellationToken))
            {
                _logger.LogError("Error in adding new supplier : {SupplierName}", supplier.SupplierName);
                return ResponseResult<bool>.BadRequest("Error adding new Supplier");
            }
            return ResponseResult<bool>.OK(MessageConstants.Message("supplier", MessageOperationType.Create));
        }
    }
}