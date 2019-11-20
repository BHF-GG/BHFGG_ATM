using BHFGG_ATM.Classes;
using System;
using System.Collections.Generic;

namespace BHFGG_ATM.EventArgClasses
{
    public class DataFormattedEventArgs : EventArgs
    {
        public List<Track> DataFormatted { get; set; }
    }
}
