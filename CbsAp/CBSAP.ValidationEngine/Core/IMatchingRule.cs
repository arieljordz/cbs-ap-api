using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSAP.ValidationEngine.Core
{
    public interface IMatchingRule<T1,T2>
    {
        bool IsMatch(T1 t1, T2 t2);
    }
}
