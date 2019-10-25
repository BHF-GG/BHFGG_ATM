using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.EventArgClasses;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    public abstract class Display : IDisplay
    {
        private List<Track> ListOfTracksToDisplay;
        private List<Condition> ListOfConditionsToDisplay;

        public void DisplayTracks(List<Track> trackList)
        {
            foreach (var track in trackList)
            {
                Console.WriteLine($"Tag: {track.Tag}          X-coordinate: {track.PositionX}          Y-coordinate: {track.PositionY}          Altitude: {track.Altitude}          Timestamp: {track.Timestamp}");
            }
        }

        public abstract void DisplayConditions(List<Condition> conditionList);

        protected Display(IFilter filteredData, IConditionChecker conditionData)
        {
            filteredData.DataFilteredEvent += HandleDataFilteredEvent;
            conditionData.ConditionsCheckedEvent += HandleConditionCheckedEvent;
        }

        private void HandleConditionCheckedEvent(object sender, ConditionCheckedEventArgs e)
        {

            //Handle message/event from conditionchecked
            ListOfConditionsToDisplay = e.ConditionsChecked;
        }

        private void HandleDataFilteredEvent(object sender, DataFilteredEventArgs e)
        {
            //Handle event from DataFiltered
            ListOfTracksToDisplay = e.DataFiltered;
        }
    }
}
