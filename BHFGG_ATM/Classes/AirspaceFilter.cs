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
        #region Attributes and Properties

        public List<Track> CurrentListOfTracks;

        private double LowerBoundary = 500;
        private double UpperBoundary = 20000;
        private double NorthBoundary = 90000;
        private double SouthBoundary = 10000;
        private double EastBoundary = 90000;
        private double WestBoundary = 10000;
        #endregion

        #region Constructor and methods for class
        public AirspaceFilter(IStringFormatter stringFormatter) //Could add all boundaries as parameters, if you wish to be able to change the airspace area
        {
            CurrentListOfTracks = new List<Track>();
            stringFormatter.DataFormattedEvent += HandleDataFormattedEvent;
        }


        public void FilterData(List<Track> trackList)
        {
            //Make an empty current list here??????
   
            //Filtering data here
            foreach (var track in trackList)
            {
                if (track.PositionX <= EastBoundary && track.PositionX >= WestBoundary &&
                    track.PositionY <= NorthBoundary && track.PositionY >= SouthBoundary &&
                    track.Altitude >= LowerBoundary && track.Altitude <= UpperBoundary)
                {
                    CurrentListOfTracks.Add(track);
                }
            }
            OnDataFilteredEvent(new DataFilteredEventArgs { DataFiltered = CurrentListOfTracks });
        }
        #endregion

        #region Event necessities
        public event EventHandler<DataFilteredEventArgs> DataFilteredEvent;

        protected virtual void OnDataFilteredEvent(DataFilteredEventArgs e)
        {
            DataFilteredEvent?.Invoke(this, e);
        }

        //Handling the event from DataFormatter setting current list of track equal to the received list of tracks
        private void HandleDataFormattedEvent(object sender, DataFormattedEventArgs e)
        {
            FilterData(e.DataFormatted);
        }
        #endregion
    }
}
