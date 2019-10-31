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
        private List<Condition> _tempCurrentCondition;
        private List<Condition> _tempCondition;

        private List<int> _newToDelete;
        private List<int> _currentToDelete;

        public event EventHandler<ConditionCheckedEventArgs> ConditionsCheckedEvent;

        public ConditionChecker(int minimumDistance, int minimumAltitude, IFilter filter)
        {
            _minimumAltitude = minimumAltitude;
            _minimumDistance = minimumDistance;
            _currentConditions = new List<Condition>(); 
            _newConditions = new List<Condition>();
            _newToDelete = new List<int>();
            _currentToDelete = new List<int>();

            _tempCondition = new List<Condition>();
            _tempCurrentCondition = new List<Condition>();

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
        

        //public void CheckCondition(List<Track> tracks = null)
        //{
        //    if (tracks != null)
        //        _tracks = tracks;

        //    // Only for debugging purpose
        //    if (_tracks.Count > 1)
        //    { Console.WriteLine("1");}

        //    _newConditions.Clear();

        //    foreach (var t in _tracks)
        //    {
        //        for (int i = _tracks.IndexOf(t); i < _tracks.Count; ++i)
        //        {
        //            if ((!DistanceOk(t, _tracks.ElementAt(i))) && (t.Tag != _tracks.ElementAt(i).Tag))
        //                _newConditions.Add(new Separation(t, _tracks.ElementAt(i), 0, new LogSeparationCondition()));
        //        }
        //    }

        //    _newToDelete.Clear();
        //    _currentToDelete.Clear();
        //    foreach (var currentC in _currentConditions)
        //    {
        //        bool keep = false;
        //        var currentS = (Separation) currentC;
        //        foreach (var newC in _newConditions)
        //        {
        //            var newS = (Separation) newC;
        //            if (((newS.Tag1 == currentS.Tag1) || (newS.Tag1 == currentS.Tag2)) &&
        //                ((newS.Tag2 == currentS.Tag1) || (newS.Tag2 == currentS.Tag2)))
        //            {
        //                _newToDelete.Add(_newConditions.IndexOf(newC));
        //                keep = true;
        //                break;
        //            }
        //            else
        //            {
        //                keep = false;
        //            }
        //        }
        //        if (!keep)
        //            _currentToDelete.Add(_currentConditions.IndexOf(currentC));
        //    }

        //    _newToDelete.Sort((x, y) => y.CompareTo(x));
        //    _newToDelete.Reverse();
        //    _currentToDelete.Sort((x, y) => y.CompareTo(x));
        //    _currentToDelete.Reverse();
        //    /*
        //    foreach (var i in _newToDelete)
        //    {
        //        _newConditions.RemoveAt(_newToDelete.ElementAt(i));
        //    }*/
        //    if (_newConditions.Count > 0)
        //    {
        //        for (int i = 0; i < _newToDelete.Count; i++)
        //        {
        //            if (_newConditions.Count < _newToDelete[i])
        //                break;
        //            _newConditions.RemoveAt(_newToDelete[i]);
        //        }
        //    }
            
        //    /*
        //    foreach (var i in _currentToDelete)
        //    {
        //        _currentConditions.RemoveAt(_currentToDelete.ElementAt(i));
        //    }
        //    */
        //    if (_currentConditions.Count>0)
        //    {
        //        for (int i = 0; i < _currentToDelete.Count; i++)
        //        {
        //            if (_currentConditions.Count < _currentToDelete[i])
        //                break;
        //            _currentConditions[_currentToDelete[i]].LogOnDestruction();
        //            _currentConditions.RemoveAt(_currentToDelete[i]);
        //        }
        //    }

        //    foreach (var newC in _newConditions)
        //    {
        //        var newS = (Separation)newC;
        //        _currentConditions.Add(new Separation(newS.Track1,newS.Track2,++_conditionId,new LogSeparationCondition(),true));
        //    }

        //    OnConditionCheckedEvent(new ConditionCheckedEventArgs { ConditionsChecked = _currentConditions });
        //}

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


        //#region CheckConditionHelp

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
                if ((newS.Tag1 == t1.Tag || newS.Tag1 == t2.Tag) && (newS.Tag2 == t1.Tag || newS.Tag2 == t2.Tag))
                    return true;
            }

            return false;
        }
        
        private void ValidateConditions()
        {
            _tempCurrentCondition.Clear();
            foreach (var currentC in _currentConditions)
            {
                bool remove = false;
                var currentS = (Separation)currentC;
                _tempCondition.Clear();
                foreach (var newC in _newConditions)
                {
                    var newS = (Separation)newC;
                    if (((newS.Tag1 == currentS.Tag1) || (newS.Tag1 == currentS.Tag2)) &&
                        ((newS.Tag2 == currentS.Tag1) || (newS.Tag2 == currentS.Tag2)))
                    {
                        _tempCondition.Add(newC);
                        remove = false;
                    }
                    else
                    {
                        remove = true;
                    }
                }
                foreach (var condition in _tempCondition)
                {
                    if (_newConditions.Contains(condition))
                        _newConditions.Remove(condition);
                }

                if (remove)
                {
                    _tempCurrentCondition.Add(currentC);
                }
            }

            foreach (var condition in _tempCurrentCondition)
            {
                if (_currentConditions.Contains(condition))
                    _currentConditions.Remove(condition);
            }

            foreach (var newC in _newConditions)
            {
                var newS = (Separation)newC;
                _currentConditions.Add(new Separation(newS.Track1, newS.Track2, ++_conditionId, new LogSeparationCondition(), true));
            }
        }
    }

}
