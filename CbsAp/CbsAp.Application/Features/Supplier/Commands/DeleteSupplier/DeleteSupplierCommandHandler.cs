using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Shared;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Supplier.Commands.DeleteSupplier
{
    public class DeleteSupplierCommandHandler : ICommandHandler<DeleteSupplierCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IDbSetDependencyChecker _modelDependencyChecker;
        private readonly ILogger<DeleteSupplierCommandHandler> _logger;

        public DeleteSupplierCommandHandler(
            IUnitofWork unitofWork,
            IDbSetDependencyChecker modelDependencyChecker, 
            ILogger<DeleteSupplierCommandHandler> logger)
        {
            _unitofWork = unitofWork;
            _modelDependencyChecker = modelDependencyChecker;
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            if (!await IsSupplierExist(request.SupplierInfoID))
            {
                _logger.LogError("Supplier with : {SupplierInfoID} is not existed", request.SupplierInfoID);
                return ResponseResult<bool>.BadRequest("Supplier is not existed");
            }
            var supplierDepedencyChecks = await _modelDependencyChecker
                .HasDependenciesAsync<SupplierInfo>(request.SupplierInfoID);

            var responseForDeleteAction = ForDeletionChecker
               .DependencyCheckerResponseResult(supplierDepedencyChecks, "Supplier");

            if (!responseForDeleteAction.IsSuccess)
                return responseForDeleteAction;

            if (!await DeleteSupplier(request.SupplierInfoID, cancellationToken))
            {
                _logger.LogError("Error in deleting supplier : {SupplierName}", request.SupplierInfoID);
                return ResponseResult<bool>.BadRequest("Error on Deleting Supplier");
            }
            return ResponseResult<bool>.OK(MessageConstants.Message("supplier", MessageOperationType.Delete));
        }

        private async Task<bool> IsSupplierExist(long supplierInfoID)
        {
            return await _unitofWork.GetRepository<SupplierInfo>()
               .AnyAsync(e => e.SupplierInfoID == supplierInfoID);
        }

        private async Task<bool> DeleteSupplier(long supplierInfoID, CancellationToken cancellationToken)
        {
            var supplier = await _unitofWork.GetRepository<SupplierInfo>()
               .ApplyPredicateAsync(e => e.SupplierInfoID == supplierInfoID);

            if (supplier.Any())
            {
                var supplierForDeletion = supplier.SingleOrDefault()!;
                await _unitofWork.GetRepository<SupplierInfo>()
                .DeleteAsync(supplierForDeletion);
                return await _unitofWork.SaveChanges(cancellationToken);
            }
            return false;
        }
    }
}