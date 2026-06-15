using CbsAp.Application.DTOs.PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Abstractions.Services.PO
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrderHeaderDto?> GetPurchaseOrderByIdAsync(long purchaseOrderID);
    }
}
