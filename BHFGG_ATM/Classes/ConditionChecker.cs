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
        private List<Condition> _conditions;
        public event EventHandler<ConditionCheckedEventArgs> ConditionsCheckedEvent;

        public ConditionChecker(IFilter filter)
        {
            _conditions = new List<Condition>();
            filter.DataFilteredEvent += HandleDataFilteredEvent;
        }

        public void CheckCondition(List<Track> tracks)
        {
            foreach (var VARIABLE in tracks)
            {
                
            }
        }

        public int GetDistance(Track t1, Track t2)
        {
            var deltaX = t1.PositionX - t2.PositionX;
            var deltaY = t1.PositionY - t2.PositionY;

            int result = Convert.ToInt32(Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2)));

            if (result < 0)
                result = result * (-1);
            return result;
        }

        private void OnConditionCheckedEvent(ConditionCheckedEventArgs e)
        {
            ConditionsCheckedEvent?.Invoke(this,e);
        }

        private void HandleDataFilteredEvent(object s, DataFilteredEventArgs e)
        {
            CheckCondition(e.DataFiltered);
        }

        
    }
}
