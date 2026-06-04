using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CbsAp.Application.Features.Supplier.Commands.UpdateSupplier;
using FluentValidation;

namespace CbsAp.Application.Features.Supplier.Commands.UpdateSupplierBankAccount
{
    public class UpdateSupplierBankAccountValidator : AbstractValidator<UpdateSupplierBankAccountCommand>
    {
        public UpdateSupplierBankAccountValidator()
        {
        }
    }
}
