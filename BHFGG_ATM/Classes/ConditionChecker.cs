using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.EventArgClasses;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    public class ConditionChecker : IConditionChecker
    {
        private List<Track> tracks;
        public event EventHandler<ConditionCheckedEventArgs> ConditionsCheckedEvent;

        public ConditionChecker()
        {
            tracks = new List<Track>();
        }

        public void CheckCondition(List<Track> tracks)
        {
            foreach (var VARIABLE in tracks)
            {
                
            }
        }

        public double GetDistance(Track t1, Track t2)
        {
            var deltaX = t1.PositionX - t2.PositionX;
            var deltaY = t1.PositionY - t2.PositionY;

            return Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
        }

        
    }
}
