using BHFGG_ATM.Classes;
using BHFGG_ATM.EventArgClasses;
using System;
using System.Collections.Generic;

namespace BHFGG_ATM.Interfaces
{
    public interface IConditionChecker
    {
        void CheckCondition(List<Track> tracks);
        event EventHandler<ConditionCheckedEventArgs> ConditionsCheckedEvent;
    }
}
