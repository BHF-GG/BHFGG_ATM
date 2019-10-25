using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Classes;
using BHFGG_ATM.EventArgClasses;

namespace BHFGG_ATM.Interfaces
{
    public interface IFilter
    {
        void FilterData(List<Track> trackList);
        event EventHandler<DataFilteredEventArgs> DataFilteredEvent;
    }
}
