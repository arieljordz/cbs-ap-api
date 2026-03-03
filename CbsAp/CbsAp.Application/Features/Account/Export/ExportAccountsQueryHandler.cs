using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.Accounts;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using AccountEntity = CbsAp.Domain.Entities.Invoicing.Account;

namespace CbsAp.Application.Features.Account.Export
{
    public class ExportAccountsQueryHandler : IQueryHandler<ExportAccountsQuery, ResponseResult<byte[]>>
    {
        private readonly IExcelService _excelService;
        private readonly IAccountRepository _accountRepository;

        public ExportAccountsQueryHandler(IExcelService excelService, IAccountRepository accountRepository)
        {
            _excelService = excelService;
            _accountRepository = accountRepository;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportAccountsQuery request, CancellationToken cancellationToken)
        {
            // Build dynamic predicate based on filter criteria
            ExpressionStarter<AccountEntity> predicate = PredicateBuilder.New<AccountEntity>(true);

            if (request.AccountID.HasValue)
            {
                predicate = predicate.And(a => a.AccountID == request.AccountID.Value);
            }

            if (!string.IsNullOrEmpty(request.AccountName))
            {
                predicate = predicate.And(a => a.AccountName != null && 
                                             a.AccountName.Contains(request.AccountName));
            }

            if (!string.IsNullOrEmpty(request.EntityName))
            {
                predicate = predicate.And(a => a.EntityProfile != null && 
                                             a.EntityProfile.EntityName != null && 
                                             a.EntityProfile.EntityName.Contains(request.EntityName));
            }

            if (request.Active.HasValue)
            {
                predicate = predicate.And(a => a.IsActive == request.Active.Value);
            }

            // Execute query with projections for optimal performance
            var accounts = await _accountRepository
                .GetAccountsAsQueryable()
                .Include(a => a.EntityProfile)
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate)
                .Select(a => new AccountExportDto
                {
                    AccountID = a.AccountID,
                    AccountName = a.AccountName ?? string.Empty,
                    EntityName = a.EntityProfile != null ? a.EntityProfile.EntityName : string.Empty,
                    Active = a.IsActive ? "Yes" : "No",
                    Dimension1 = a.Dimension1,
                    Dimension2 = a.Dimension2,
                    Dimension3 = a.Dimension3,
                    Dimension4 = a.Dimension4,
                    Dimension5 = a.Dimension5,
                    Dimension6 = a.Dimension6,
                    Dimension7 = a.Dimension7,
                    Dimension8 = a.Dimension8,
                    IsTaxCodeMandatory = a.IsTaxCodeMandatory.HasValue && a.IsTaxCodeMandatory.Value ? "Yes" : "No"
                })
                .ToListAsync(cancellationToken);

            // Check if any results were found
            if (accounts == null || accounts.Count == 0)
            {
                return ResponseResult<byte[]>.NotFound(
                    MessageConstants.Message("Account", MessageOperationType.NotFound));
            }

            // Generate Excel file asynchronously
            var excelBytes = await Task.Run(
                () => _excelService.GenerateExcel(accounts, "Accounts"),
                cancellationToken);

            return ResponseResult<byte[]>.Success(excelBytes, "Export excel data");
        }
    }
}
