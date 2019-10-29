using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.EventArgClasses;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    public class Display : IDisplay
    {
        public List<Track> ListOfTracksToDisplay { get; set; }

        public void DisplayTracks(List<Track> trackList)
        {
            foreach (var track in trackList)
            {
                Console.WriteLine($"Tag: {track.Tag}          X-coordinate: {track.PositionX}          Y-coordinate: {track.PositionY}          Altitude: {track.Altitude}          Timestamp: {track.Timestamp}" +
                                  $"          Horizontal Velocity: {track.HorizontalVelocity}          Current Compass course: {track.HorizontalVelocity}");
            }
        }

        public virtual void DisplayConditions(List<Condition> conditionList) { }

        public Display(IFilter filteredData)
        {
            filteredData.DataFilteredEvent += HandleDataFilteredEvent;
            ListOfTracksToDisplay = new List<Track>();
        }

        private void HandleDataFilteredEvent(object sender, DataFilteredEventArgs e)
        {
            //Handle event from DataFiltered
            ListOfTracksToDisplay = e.DataFiltered;
            DisplayTracks(ListOfTracksToDisplay);
        }
    }
}
