using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.AdvanceSearch;
using CbsAp.Application.Features.Invoicing.InvActions.Queries.MyInvoiceSearch;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.Application.Features.AdvanceSearch.Commands.CreateAdvanceSearch
{
    public record CreateAdvanceSearchCommand(AdvanceSearchRequestForm advanceSearch, string CreatedBy) : ICommand<ResponseResult<long>>
    {
    }
}
