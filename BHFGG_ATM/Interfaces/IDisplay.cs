using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Classes;

namespace BHFGG_ATM.Interfaces
{
    public interface IDisplay
    {
        void DisplayTracks(List<Track> trackList);
        void DisplayConditions(List<Condition> conditionList);
    }
}
