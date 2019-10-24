using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Classes;

namespace BHFGG_ATM.EventArgClasses
{
    public class DataFilteredEventArgs : EventArgs
    {
        //public List<Track> DataFiltered
        //{
        //    get { return DataFiltered; }
        //    set { DataFiltered = value; }
        //}
        public List<Track> DataFiltered { get; set; }
    }
}
