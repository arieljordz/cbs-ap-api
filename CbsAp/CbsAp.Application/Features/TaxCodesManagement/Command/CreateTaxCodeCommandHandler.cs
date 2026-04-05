using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Features.TaxCodes.Command.Common;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.TaxCodes;
using Mapster;

namespace CbsAp.Application.Features.TaxCodesManagement.Command
{
    public class CreateTaxCodeCommandHandler : ICommandHandler<CreateTaxCodeCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;

        public CreateTaxCodeCommandHandler(IUnitofWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public async Task<ResponseResult<string>> Handle(CreateTaxCodeCommand request, CancellationToken cancellationToken)
        {
            var taxCode = request.Adapt<TaxCode>();
            await _unitOfWork.GetRepository<TaxCode>().AddAsync(taxCode);
            bool success = await _unitOfWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
            if (!success)
            {
                return ResponseResult<string>.BadRequest(
                   MessageConstants.FormatMessage(MessageConstants.AddError, "taxcode"));
            }

            return ResponseResult<string>.Created(request.Code,
                       MessageConstants.FormatMessage(MessageConstants.AddSuccess, request.Code));
        }
    }
}