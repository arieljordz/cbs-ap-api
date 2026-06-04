using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Supplier.Commands.UpdateSupplierBankAccount
{
    public record UpdateSupplierBankAccountCommand(SupplierBankAccountDto dto, string updateBy)
        :ICommand<ResponseResult<List<SupplierBankAccountDto>>>
    {
    }
}
