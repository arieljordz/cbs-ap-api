using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CbsAp.Application.Features.Supplier.Commands.DeleteSupplier
{
    public record DeleteSupplierCommand(long SupplierInfoID): ICommand<ResponseResult<bool>>
    {
    }
}
