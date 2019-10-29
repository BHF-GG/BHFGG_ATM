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
        private int _conditionId;

        private List<Condition> _conditions;
        public event EventHandler<ConditionCheckedEventArgs> ConditionsCheckedEvent;

        public ConditionChecker(int minimumDistance, int minimumAltitude, IFilter filter)
        {
            _minimumAltitude = minimumAltitude;
            _minimumDistance = minimumDistance;
            _conditions = new List<Condition>();
            _conditionId = 0;
            filter.DataFilteredEvent += HandleDataFilteredEvent;
        }

        public void CheckCondition(List<Track> tracks)
        {
            //_conditions.Clear();
            var validCondition = true;
            foreach (var track in tracks)
            {
                foreach (var t in tracks.Where(t => !DistanceOk(track, t) && track.Tag != t.Tag))
                {
                    foreach (var separation in _conditions.Where(condition => condition.Type == "Separation")
                        .Select(condition => (Separation) condition).Where(separation => 
                            ((separation.Tag1 == track.Tag || separation.Tag2 == track.Tag) &&
                             (separation.Tag1 == t.Tag || separation.Tag2 == t.Tag))))
                    {
                        validCondition = false;
                    }

                    if (validCondition)
                    {
                        //if (track.Tag == t.Tag)
                          //  break;
                        _conditions.Add(new Separation(track, t, ++_conditionId, new LogSeparationCondition()));
                    }
                        
                    validCondition = true;
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
