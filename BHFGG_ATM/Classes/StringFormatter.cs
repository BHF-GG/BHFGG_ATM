﻿using BHFGG_ATM.EventArgClasses;
using BHFGG_ATM.Interfaces;
using System;
using System.Collections.Generic;
using TransponderReceiver;
using ITransponderReceiver = TransponderReceiver.ITransponderReceiver;

namespace BHFGG_ATM.Classes
{

    public class StringFormatter : IStringFormatter
    {
        private ICompassCourseCalculator _courseCalculator;
        private IVelocityCalculator _velocityCalculator;

        public StringFormatter(ITransponderReceiver transponderReceiver, ICompassCourseCalculator courseCalculator, IVelocityCalculator velocityCalculator)
        {
            transponderReceiver.TransponderDataReady += HandleTransponderDataEvent;

            _courseCalculator = courseCalculator;
            _velocityCalculator = velocityCalculator;

        }

        #region Event receiver


        public List<string> CurrentTransponderData { get; set; }

        private void HandleTransponderDataEvent(object sender, RawTransponderDataEventArgs e)
        {
            CurrentTransponderData = e.TransponderData;
            FormatData(CurrentTransponderData);
        }


        #endregion

        #region Event source

        private List<Track> _previousListTracks = new List<Track>();
        public event EventHandler<DataFormattedEventArgs> DataFormattedEvent;

        public void FormatData(List<string> stringToFormat)
        {

            List<Track> newListOfTrack = new List<Track>();

            foreach (var dataString in stringToFormat)
            {
                string[] sArray = dataString.Split(';');
                Track track = new Track();

                track.Tag = sArray[0];
                track.PositionX = Convert.ToDouble(sArray[1]);
                track.PositionY = Convert.ToDouble(sArray[2]);
                track.Altitude = Convert.ToDouble(sArray[3]);
                track.Timestamp = sArray[4];

                foreach (var pt in _previousListTracks)
                {
                    if (track.Tag == pt.Tag)
                    {
                        track.CompassCourse = _courseCalculator.CalculateCompassCourse(
                            pt.PositionX, pt.PositionY, track.PositionX, track.PositionY);
                        track.HorizontalVelocity = _velocityCalculator.CalculateCurrentVelocity(
                            pt.PositionX, pt.PositionY, track.PositionX, track.PositionY, pt.Timestamp, track.Timestamp);
                    }
                }

                newListOfTrack.Add(track);
            }

            _previousListTracks = newListOfTrack;
            OnDataFormattedEvent(new DataFormattedEventArgs { DataFormatted = newListOfTrack });
        }

        protected virtual void OnDataFormattedEvent(DataFormattedEventArgs e)
        {
            DataFormattedEvent?.Invoke(this, e);
        }


        #endregion
    }
}
