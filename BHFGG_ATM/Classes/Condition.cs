using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    public abstract class Condition
    {
        public string Type { get; protected set; }
        public string Timestamp { get; protected set; }
        protected ILogCondition LogCondition;
        protected virtual void LogOnConstruction()
        {
            LogCondition.LogCondition(this);
        }
        protected virtual void LogOnDestruction()
        {
            LogCondition.LogEndCondition(this);
        }

    }
}
