using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;



namespace CbsAp.Application.Features.Invoicing.InvActions.Command.Validate

{
    public record ValidateByIdsCommand(
        List<long> InvoiceIds,
        string UpdatedBy
        
        ) : ICommand<ResponseResult<List<InvValidationResponseDto>>>;


}