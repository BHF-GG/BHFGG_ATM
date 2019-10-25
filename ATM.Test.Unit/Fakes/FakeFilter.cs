using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Classes;
using BHFGG_ATM.EventArgClasses;
using BHFGG_ATM.Interfaces;

namespace ATM.Test.Unit.Fakes
{
    internal class FakeFilter: IFilter
    {
        public void FilterData(List<Track> trackList)
        {
            DataFilteredEvent?.Invoke(
                this,
                new DataFilteredEventArgs() {DataFiltered = trackList});
        }

        public event EventHandler<DataFilteredEventArgs> DataFilteredEvent;
    }
}
