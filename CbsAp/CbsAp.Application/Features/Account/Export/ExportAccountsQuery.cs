using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Account.Export
{
    public record ExportAccountsQuery(
            long? AccountID,
            string? AccountName,
            string? EntityName,
            bool? Active) : IQuery<ResponseResult<byte[]>>
    {
    }
}
