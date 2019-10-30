using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        private List<Track> _tracks;
        private List<Condition> _currentConditions;
        private List<Condition> _newConditions;


        public event EventHandler<ConditionCheckedEventArgs> ConditionsCheckedEvent;

        public ConditionChecker(int minimumDistance, int minimumAltitude, IFilter filter)
        {
            _minimumAltitude = minimumAltitude;
            _minimumDistance = minimumDistance;
            _currentConditions = new List<Condition>();
            _newConditions = new List<Condition>();
            _conditionId = 0;
            filter.DataFilteredEvent += HandleDataFilteredEvent;
        }

        public void CheckCondition(List<Track> tracks= null)
        {
            if (tracks != null)
                _tracks = tracks;
            GetConditions();

            ValidateConditions();
            
            OnConditionCheckedEvent(new ConditionCheckedEventArgs{ConditionsChecked = _currentConditions});
        }
        /*
        public void CheckConditions(List<Track> tracks)
        {
            _newConditions.Clear();
            foreach (var track in tracks)
            {
                foreach (var t in tracks)
                {
                    if (track.Tag != t.Tag)
                    {
                        if (!DistanceOk(track, t))
                        {
                            foreach (var cond in _currentConditions)
                            {
                                var s = (Separation) cond;
                                if (!(track.Tag == s.Tag1 || track.Tag == s.Tag2) && (t.Tag == s.Tag1 ||t.Tag == s.Tag2))
                                    _newConditions.Add(new Separation(track,t,1,new LogSeparationCondition(),true));
                            }
                        }
                    }
                }
            }
            OnConditionCheckedEvent(new ConditionCheckedEventArgs { ConditionsChecked = _newConditions });
        }*/

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
            _tracks = e.DataFiltered;
            CheckCondition();
        }


        #region CheckConditionHelp

        private void GetConditions()
        {
            _newConditions.Clear();
            var validCondition = true;
            foreach (var track in _tracks)
            {
                foreach (var t in _tracks.Where(t => !DistanceOk(track, t) && track.Tag != t.Tag))
                {
                    if (!CheckTracks(track,t))
                        _newConditions.Add(new Separation(track, t, 0, new LogSeparationCondition()));
                }
            }
        }

        private bool CheckTracks(Track t1, Track t2)
        {
            foreach (var newC in _newConditions)
            {
                var newS = (Separation) newC;
                if ((newS.Tag1 == t1.Tag || newS.Tag2 == t2.Tag) && (newS.Tag2 == t1.Tag || newS.Tag2 == t2.Tag))
                    return true;
            }

            return false;
        }

        private void ValidateConditions()
        {
            foreach (var currentC in _currentConditions)
            {
                bool remove = false;
                var currentS = (Separation)currentC;
                foreach (var newC in _newConditions)
                {
                    var newS = (Separation)newC;
                    if (((newS.Tag1 == currentS.Tag1) || (newS.Tag1 == currentS.Tag2)) &&
                        ((newS.Tag2 == currentS.Tag1) || (newS.Tag2 == currentS.Tag2)))
                    {
                        _newConditions.Remove(newC);
                        remove = false;
                        //break;
                    }
                    else
                    {
                        remove = true;
                    }
                }

                if (remove)
                {
                    _currentConditions.Remove(currentC);
                }
            }

            foreach (var newC in _newConditions)
            {
                var newS = (Separation)newC;
                _currentConditions.Add(new Separation(newS.Track1, newS.Track2, ++_conditionId, new LogSeparationCondition(), true));
            }
        }

        #endregion

    }
}
