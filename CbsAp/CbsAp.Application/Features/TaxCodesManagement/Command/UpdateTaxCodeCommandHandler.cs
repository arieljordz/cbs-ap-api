using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Features.TaxCodes.Command.Common;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.TaxCodes;
using Mapster;

namespace CbsAp.Application.Features.TaxCodesManagement.Command
{
    public class UpdateTaxCodeCommandHandler : ICommandHandler<UpdateTaxCodeCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;

        public UpdateTaxCodeCommandHandler(IUnitofWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public async Task<ResponseResult<string>> Handle(UpdateTaxCodeCommand request, CancellationToken cancellationToken)
        {
            var taxCodeRepository = _unitOfWork.GetRepository<TaxCode>();

            var taxCodeQuery = await taxCodeRepository.ApplyPredicateAsync(x => x.TaxCodeID == request.TaxCodeID);
            var taxCodeEntity = taxCodeQuery.FirstOrDefault();

            if (taxCodeEntity == null)
            {
                return ResponseResult<string>.NotFound("TaxCode not found.");
            }

            var updatedTaxCode = request.Adapt<TaxCode>();
            updatedTaxCode.CreatedBy = taxCodeEntity?.CreatedBy;
            updatedTaxCode.CreatedDate = taxCodeEntity?.CreatedDate;
            updatedTaxCode.LastUpdatedDate = DateTime.UtcNow;
            updatedTaxCode.LastUpdatedBy = request.LastUpdatedBy;

            await taxCodeRepository.UpdateAsync(updatedTaxCode.TaxCodeID, updatedTaxCode);
            bool success = await _unitOfWork.SaveChanges(string.Empty,string.Empty,cancellationToken);

            return success ?
                ResponseResult<string>.Success(MessageConstants.FormatMessage(MessageConstants.UpdateSuccess, request.Code)) :
                ResponseResult<string>.BadRequest(MessageConstants.FormatMessage(MessageConstants.UpdateError, "TaxCode"));
        }
    }
}