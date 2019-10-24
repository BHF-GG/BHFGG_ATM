using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.EventArgClasses;

namespace BHFGG_ATM.Interfaces
{
    public interface IStringFormatter
    {
        void FormatData(List<string> stringToFormat);
        event EventHandler<DataFormattedEventArgs> DataFormattedEvent;

    }
}
