using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.EventArgClasses;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    public class AirspaceFilter : IFilter
    {
        //Necessary attributes/properties here

        #region Attributes and Properties

        public List<Track> CurrentListOfTracks { get; set; }

        private double LowerBoundary = 500;
        private double UpperBoundary = 20000;
        private double NorthBoundary = 10000;
        private double SouthBoundary = 90000;
        private double EastBoundary = 10000;
        private double WestBoundary = 90000;

        #endregion

        public AirspaceFilter(IStringFormatter stringFormatter)
        {
            stringFormatter.DataFormattedEvent += HandleDataFormattedEvent;
        }

        public event EventHandler<DataFilteredEventArgs> DataFilteredEvent;

        public void FilterData(List<Track> trackList)
        {
            //Filtering data here
            foreach (var track in trackList)
            {
                if (track.PositionX >= EastBoundary && track.PositionX <= WestBoundary &&
                    track.PositionY >= NorthBoundary && track.PositionY <= SouthBoundary &&
                    track.Altitude >= LowerBoundary && track.Altitude <= UpperBoundary)
                {
                    CurrentListOfTracks.Add(track);
                }
            }
            OnDataFilteredEvent(new DataFilteredEventArgs { DataFiltered = CurrentListOfTracks });
        }


        protected virtual void OnDataFilteredEvent(DataFilteredEventArgs e)
        {
            DataFilteredEvent?.Invoke(this, e);
        }

        //Handling the event from DataFormatter setting current list of track equal to the received list of tracks
        private void HandleDataFormattedEvent(object sender, DataFormattedEventArgs e)
        {
            //CurrentListOfTracks = e.DataFormatted;
            FilterData(e.DataFormatted);
        }
    }
}
