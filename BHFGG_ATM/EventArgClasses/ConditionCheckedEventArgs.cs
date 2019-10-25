using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Classes;

namespace BHFGG_ATM.EventArgClasses
{
    public class ConditionCheckedEventArgs : EventArgs
    {
        public List<Condition> ConditionsChecked { get; set; }
    }
}
