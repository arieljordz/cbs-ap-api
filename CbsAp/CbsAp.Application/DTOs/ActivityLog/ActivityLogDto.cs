using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.ActivityLog
{
    public class ActivityLogDto
    {
        public int ActivityID { get; set; }
        public int InvoiceID { get; set; }
        public string? Activity { get; set; }
        public string? ActionBy { get; set; }
        public string? Module { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? TableName { get; set; }
        public string? ColumnName { get; set; }
        public string? metaDataOld { get; set; }
        public string? metaDataNew { get; set; }
        public string? MetaData { get; set; }
        public DateTime? ActivityDate { get; set; }
    }
}
