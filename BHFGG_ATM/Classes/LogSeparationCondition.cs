using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Interfaces;


namespace BHFGG_ATM.Classes
{
    public class LogSeparationCondition : ILogCondition
    {
        public void LogCondition(Condition c)
        {
            // Write Data to file.
        }

        public void LogEndCondition(Condition c)
        {
            // Write Data to file
        }
    }
}
