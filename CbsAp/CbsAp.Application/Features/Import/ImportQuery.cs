using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Import;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Import
{
    public record ImportQuery(List<ImportInvoiceDto> dto) : IQuery<ResponseResult<List<ImportInvoiceResponseDto>>>;
}
