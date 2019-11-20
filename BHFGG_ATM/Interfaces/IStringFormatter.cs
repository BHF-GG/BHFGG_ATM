using BHFGG_ATM.EventArgClasses;
using System;
using System.Collections.Generic;

namespace BHFGG_ATM.Interfaces
{
    public interface IStringFormatter
    {
        void FormatData(List<string> stringToFormat);
        event EventHandler<DataFormattedEventArgs> DataFormattedEvent;

    }
}
