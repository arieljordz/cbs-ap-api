using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvAllocationLine.Commands
{
    public record DeleteAllocationLineCommand(long AllocationLineID, string UpdatedBy) : ICommand<ResponseResult<bool>>
    {
    }
}