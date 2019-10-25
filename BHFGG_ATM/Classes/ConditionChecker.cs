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
        private int _minimumDistance;
        private int _minimumAltitude;

        private List<Condition> _conditions;
        public event EventHandler<ConditionCheckedEventArgs> ConditionsCheckedEvent;

        public ConditionChecker(int minimumDistance, int minimumAltitude, IFilter filter)
        {
            _minimumAltitude = minimumAltitude;
            _minimumDistance = minimumDistance;
            _conditions = new List<Condition>();
            filter.DataFilteredEvent += HandleDataFilteredEvent;
        }

        public void CheckCondition(List<Track> tracks)
        {
            _conditions = new List<Condition>();
            foreach (var track in tracks)
            {
                for (int i = 0; i < tracks.Count; i++)
                {
                   if (!DistanceOk(track, tracks[i]) && track != tracks[i])
                       _conditions.Add(new Separation(track, tracks[i],new LogSeparationCondition()));
                }
            }
            OnConditionCheckedEvent(new ConditionCheckedEventArgs{ConditionsChecked = _conditions});
        }

        private bool DistanceOk(Track t1, Track t2)
        {
            if (GetAltitudeDelta(t1, t2) > _minimumAltitude)
                return true;
            if (GetDistance(t1, t2) > _minimumDistance)
                return true;

            return false;
        }

        private int GetAltitudeDelta(Track t1, Track t2)
        {
            double delta = t1.Altitude - t2.Altitude;

            if (delta <0)
                delta = delta * (-1);
            return Convert.ToInt32(delta);
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
