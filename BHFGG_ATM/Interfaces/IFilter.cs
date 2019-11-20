using BHFGG_ATM.Classes;
using BHFGG_ATM.EventArgClasses;
using System;
using System.Collections.Generic;

namespace BHFGG_ATM.Interfaces
{
    public interface IFilter
    {
        void FilterData(List<Track> trackList);
        event EventHandler<DataFilteredEventArgs> DataFilteredEvent;
    }
}
