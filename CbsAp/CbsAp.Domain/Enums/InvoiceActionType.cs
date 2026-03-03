using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Domain.Enums
{
    public enum InvoiceActionType
    {
        Save = 1,
        Validate = 2,
        Submit = 3,
        Hold = 4,
        TakeLock = 5,
        AddComment =6,
        AddAttachments=7,
        Approve =8,
        RouteToException=9,
        Reactive=10,
        Reject=11,
        Import= 12,
    }

    public enum SourceApplication
    {
        Portal = 1,
        Agent = 2,
    }
}
