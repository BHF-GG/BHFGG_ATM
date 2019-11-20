using BHFGG_ATM.Classes;
using System.Collections.Generic;

namespace BHFGG_ATM.Interfaces
{
    public interface IDisplay
    {
        void DisplayTracks(List<Track> trackList);
        void DisplayConditions(List<Condition> conditionList);
    }
}
