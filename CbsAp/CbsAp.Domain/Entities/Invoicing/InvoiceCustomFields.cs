using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Domain.Entities.Invoicing
{
    public class FreeFieldSets
    {
        public string? FreeField1 { get; set; }
        public string? FreeField2 { get; set; }
        public string? FreeField3 { get; set; }
    }

    public class SpareAmountSets
    {
        public decimal? SpareAmount1 { get; set; }
        public decimal? SpareAmount2 { get; set; }
        public decimal? SpareAmount3 { get; set; }
    }
}
