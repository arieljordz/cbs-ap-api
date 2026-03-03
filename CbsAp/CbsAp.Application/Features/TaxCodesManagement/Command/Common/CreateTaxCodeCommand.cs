using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.TaxCodes.Command.Common
{
    public record CreateTaxCodeCommand(
        long EntityID,
        string TaxCodeName,
        string Code,
        decimal TaxRate,
        string CreatedBy
        ) :
        ICommand<ResponseResult<string>>;
}