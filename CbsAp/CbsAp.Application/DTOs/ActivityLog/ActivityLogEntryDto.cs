using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.ActivityLog
{
    public class ActivityLogEntryDto
    {
        public int InvoiceID { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? Action { get; set; }
        public string? Table { get; set; }
        public string? Column { get; set; }
        public string? ActivityClass { get; set; }
        public string? PrevValue { get; set; }
        public string? NewValue { get; set; }
        public string? ActionedBy { get; set; }
    }
}
