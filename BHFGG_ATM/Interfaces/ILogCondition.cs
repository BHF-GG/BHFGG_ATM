using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Classes;

namespace BHFGG_ATM.Interfaces
{
    public interface ILogCondition
    {
        void LogCondition(Condition c);
        void LogEndCondition(Condition c);
    }
}
