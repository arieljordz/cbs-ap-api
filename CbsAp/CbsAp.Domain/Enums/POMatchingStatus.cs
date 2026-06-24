using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Domain.Enums
{
    public enum POMatchingStatus
    {
        FullyMatched = 1,
        PartialMatched = 2,
        Unmatched = 3
    }

    public enum MatchingStatus
    {
        UnMatched = 0,
        FullyMatched = 1,
        PartiallyMatched = 2
    }

    public enum PurchaseOrderMatchType
    {
        Amount=0,
        Quantity=1
    }

    public enum InvoicePOMatchingStatus
    {
        UnMatched = 0,
        FullyMatched = 1,
        PartiallyMatched = 2
    }


    public enum POLineDeliveryStatus
    {
        NotDelivered = 0,        
        FullyDelivered= 1,
        PartiallyDelivered = 2,
    }
}
