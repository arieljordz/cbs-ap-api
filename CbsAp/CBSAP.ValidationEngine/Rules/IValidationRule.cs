using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSAP.ValidationEngine.Rules
{

    public interface IValidationRule
    {
        string Name { get; }
        EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null);

    }


}