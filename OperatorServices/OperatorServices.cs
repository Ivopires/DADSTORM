using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADSTORM
{
    public interface OperatorServices
    {
        void exchangeTuples(string[] tuples);
        void setSendAddresses(string operatorID, string sendAddress);
        void startToProcess();
    }
}
