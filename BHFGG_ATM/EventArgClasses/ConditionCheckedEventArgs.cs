using BHFGG_ATM.Classes;
using System;
using System.Collections.Generic;

namespace BHFGG_ATM.EventArgClasses
{
    public class ConditionCheckedEventArgs : EventArgs
    {
        public List<Condition> ConditionsChecked { get; set; }
    }
}
