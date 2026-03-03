using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PO.Command.SavePO
{
    public record SavePOCommand(SavePOMatchingDto SavePOMatchingDto, string CreatedBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}
