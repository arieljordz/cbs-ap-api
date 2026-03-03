using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.TaxCodes.Command.Common
{
    public record UpdateTaxCodeCommand(
        long TaxCodeID,
        long EntityID,
        string TaxCodeName,
        string Code,
        decimal TaxRate,
        string LastUpdatedBy
        ) : ICommand<ResponseResult<string>>;
}