using CbsAp.Application.DTOs.TaxCodesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Abstractions.Services.TaxCode
{
    public interface ITaxcodeService 
    {
        Task<IEnumerable<TaxCodeLookupDto>> GetTaxCodeLookupAsync();
    }
}
