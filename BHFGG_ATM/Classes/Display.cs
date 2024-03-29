﻿using BHFGG_ATM.EventArgClasses;
using BHFGG_ATM.Interfaces;
using System;
using System.Collections.Generic;

namespace BHFGG_ATM.Classes
{
    public class Display : IDisplay
    {
        public List<Track> ListOfTracksToDisplay;// { get; set; }

        //public DateTime dt;

        public void DisplayTracks(List<Track> trackList)
        {
            foreach (var track in trackList)
            {
                //dt = new VelocityCalculator().ConvertStringTimestampToDateTime(track.Timestamp);
                Console.WriteLine(
                    $"Tag: {track.Tag}\t PositionXY: ({track.PositionX}, {track.PositionY})\t Altitude: {track.Altitude} m\t Timestamp: {track.Timestamp}\t " +
                    $"Horizontal Velocity: {track.HorizontalVelocity} m/s\t Current compass course: {track.CompassCourse} degrees");
            }
        }

        public virtual void DisplayConditions(List<Condition> conditionList) { }

        public Display(IFilter filteredData)
        {
            ListOfTracksToDisplay = new List<Track>();
            filteredData.DataFilteredEvent += HandleDataFilteredEvent;
            //ListOfTracksToDisplay = new List<Track>();
        }

        private void HandleDataFilteredEvent(object sender, DataFilteredEventArgs e)
        {
            //Handle event from DataFiltered
            ListOfTracksToDisplay = e.DataFiltered;
            //DisplayTracks(ListOfTracksToDisplay); 
            DisplayTracksAndConditions();
        }

        protected virtual void DisplayTracksAndConditions()
        {
            //Console.Clear(); 
            DisplayTracks(ListOfTracksToDisplay);
        }
    }
}
