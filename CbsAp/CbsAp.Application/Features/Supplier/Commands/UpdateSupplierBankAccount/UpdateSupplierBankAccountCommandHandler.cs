using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Supplier.Commands.UpdateSupplierBankAccount
{
    public class UpdateSupplierBankAccountCommandHandler : ICommandHandler<UpdateSupplierBankAccountCommand, ResponseResult<List<SupplierBankAccountDto>>>
    {
        private readonly IUnitofWork _unitOfWork;
        public UpdateSupplierBankAccountCommandHandler(IUnitofWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public async Task<ResponseResult<List<SupplierBankAccountDto>>> Handle(UpdateSupplierBankAccountCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<SupplierBankAccount>();

      
            var accountDetails = await repo.FirstOrDefaultAsync(
                w => w.SupplierBankAccountID == request.dto.SupplierBankAccountID
            );

            if (accountDetails == null)
            {
                return ResponseResult<List<SupplierBankAccountDto>>.BadRequest("Account not found");
            }


            accountDetails.IsActive = request.dto.IsActive;
            await repo.UpdateAsync(accountDetails.SupplierBankAccountID, accountDetails);

            await _unitOfWork.SaveChanges(request.updateBy, "Supplier", cancellationToken);

 
            var allBankAccounts = await repo.GetAllAsync();
            var accounts = allBankAccounts
                .Where(w => w.SupplierInfoID == accountDetails.SupplierInfoID)
                .Select(s => new SupplierBankAccountDto
                {
                    SupplierBankAccountID = s.SupplierBankAccountID,
                    SupplierInfoID = s.SupplierInfoID,
                    BankAccountNumber = s.BankAccountNumber,
                    BankName = s.BankName,
                    IsActive = s.IsActive,
                })
                .ToList();

            // Return response
            return ResponseResult<List<SupplierBankAccountDto>>.OK(accounts);
        }
    }
}
