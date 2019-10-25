﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Classes;
using BHFGG_ATM.EventArgClasses;
using BHFGG_ATM.Interfaces;
using NUnit.Framework;

namespace ATM.Test.Unit
{
    [TestFixture]
    public class AtmUnitTest
    {

        [SetUp]
        public void SetUp()
        {

        }
    }

    [TestFixture]
    public class ConditionCheckerUnitTest
    {
        private ConditionChecker _uut;
        private ConditionCheckedEventArgs _receivedEventArgs;
        private Track _track1;
        private Track _track2;
        private List<Track> _tracks;

        [SetUp]
        public void SetUp()
        {
            _receivedEventArgs = null;
            _track1 = new Track();
            _track2 = new Track();
            _tracks = new List<Track>();
            _uut = new ConditionChecker(5000,300,new AirspaceFilter(new StringFormatter()));

            _uut.ConditionsCheckedEvent +=
                (o, args) => { _receivedEventArgs = args; };
        }


        [TestCase(400,500,10000,10001,10000,10001)]
        public void CheckCondition_GeneratesConditionList(double A1, double A2, double x1, double x2, double y1, double y2)
        {
            _track1.Altitude = A1;
            _track2.Altitude = A2;
            _track1.PositionX = x1;
            _track1.PositionY = y1;
            _track2.PositionX = x2;
            _track2.PositionY = y2;
            
            _tracks.Add(_track1);
            _tracks.Add(_track2);

            _uut.CheckCondition(_tracks);
            Assert.That(_receivedEventArgs, Is.Not.Null);
        }

        [TestCase(400, 500, 10000, 10001, 10000, 10001,1)]
        public void CheckCondition_CorrectAmmountOfConditionsGenerated(double A1, double A2, double x1, double x2, double y1, double y2, int AmmountOfConditions)
        {
            _track1.Altitude = A1;
            _track2.Altitude = A2;
            _track1.PositionX = x1;
            _track1.PositionY = y1;
            _track2.PositionX = x2;
            _track2.PositionY = y2;

            _tracks.Add(_track1);
            _tracks.Add(_track2);

            _uut.CheckCondition(_tracks);
            Assert.That(_receivedEventArgs.ConditionsChecked.Count, Is.EqualTo(AmmountOfConditions));
        }

        [TestCase(400, 500, 10000, 10001, 10000, 10001)]
        public void CheckCondition_CorrectConditionGenerated(double A1, double A2, double x1, double x2, double y1, double y2)
        {
            _track1.Altitude = A1;
            _track2.Altitude = A2;
            _track1.PositionX = x1;
            _track1.PositionY = y1;
            _track2.PositionX = x2;
            _track2.PositionY = y2;

            _tracks.Add(_track1);
            _tracks.Add(_track2);

            _uut.CheckCondition(_tracks);
            Assert.That(_receivedEventArgs.ConditionsChecked.ElementAt(0).Type, Is.EqualTo("Separation"));
        }

        [TestCase(400, 500, 10000, 10001, 10000, 10001)]
        public void CheckCondition_CorrectDataInSeparation(double A1, double A2, double x1, double x2, double y1, double y2)
        {
            _track1.Tag = "Track1";
            _track2.Tag = "Track2";
            _track1.Altitude = A1;
            _track2.Altitude = A2;
            _track1.PositionX = x1;
            _track1.PositionY = y1;
            _track2.PositionX = x2;
            _track2.PositionY = y2;

            _tracks.Add(_track1);
            _tracks.Add(_track2);

            _uut.CheckCondition(_tracks);

            Separation separationToCheck = (Separation)_receivedEventArgs.ConditionsChecked.ElementAt(0);
            Assert.That(separationToCheck.Tag1, Is.EqualTo("Track1"));
            Assert.That(separationToCheck.Tag2, Is.EqualTo("Track2"));
        }

        [TestCase(0, 0, 300, 100, 316)]
        [TestCase(300, 100, 0, 0, 316)]
        [TestCase(-300, -100, 0, 0, 316)]
        [TestCase(100, 100, 400, -300, 500)]
        [TestCase(-100, -200, -200, -1000, 806)]
        [TestCase(50, -40, 50, -40, 0)]
        [TestCase(10000, 10000, 90000, 90000, 113137)]
        public void GetDistance_ReturnsCorrectDistanceBetweenTwoTracks(double x1, double y1, double x2, double y2, int result)
        {
            _track1.PositionX = x1;
            _track1.PositionY = y1;
            _track2.PositionX = x2;
            _track2.PositionY = y2;

            Assert.That(_uut.GetDistance(_track1,_track2).Equals(result));
        }
    }

    [TestFixture]

}
