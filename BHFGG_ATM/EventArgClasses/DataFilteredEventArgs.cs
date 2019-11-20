using BHFGG_ATM.Classes;
using System;
using System.Collections.Generic;

namespace BHFGG_ATM.EventArgClasses
{
    public class DataFilteredEventArgs : EventArgs
    {
        public List<Track> DataFiltered { get; set; }
    }
}
