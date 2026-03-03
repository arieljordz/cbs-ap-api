using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvAllocationLine.Commands
{
    public record AddAllocationLineCommand(InvAllocLineDto invoiceDto, string CreatedBy) : ICommand<ResponseResult<long>>
    {
    }
}