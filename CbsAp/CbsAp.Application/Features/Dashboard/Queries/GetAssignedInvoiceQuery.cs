using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Dashboard;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.Dashboard.Queries
{
    public record GetAssignedInvoiceQuery(string CurrentUser,long RoleId,string FilterType) : IQuery<ResponseResult<AssignedInvoiceResultDTO>>;
}
