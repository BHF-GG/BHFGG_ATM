using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.EventArgClasses;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    public class StringFormatter : IStringFormatter
    {
        public void FormatData(List<string> stringToFormat)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<DataFormattedEventArgs> DataFormattedEvent;

    }
}
