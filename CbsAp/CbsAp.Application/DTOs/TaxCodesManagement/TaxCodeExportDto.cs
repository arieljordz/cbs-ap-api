using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.TaxCodesManagement
{
    public class TaxCodeExportDto
    {
        public string EntityName { get; set; } = string.Empty;
        public string TaxCodeName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal TaxRate { get; set; }
    }
}
