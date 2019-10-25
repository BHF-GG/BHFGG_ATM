using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ATM.Test.Unit.Fakes;
using BHFGG_ATM.Classes;
using BHFGG_ATM.EventArgClasses;
using BHFGG_ATM.Interfaces;
using NSubstitute;
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
        //private IFilter _testFilterSource;
        private readonly IFilter _fakeFilter = new FakeFilter();


        [SetUp]
        public void SetUp()
        {
            //_testFilterSource = Substitute.For<IFilter>();
            _receivedEventArgs = null;
            _track1 = new Track();
            _track1.Tag = "1";
            _track2 = new Track();
            _track2.Tag = "2";
            _tracks = new List<Track>();

            _uut = new ConditionChecker(5000,300, _fakeFilter);

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
        
        [TestCase(400, 500, 10000, 10001, 10000, 10001,3)]
        [TestCase(500, 900, 10000, 10001, 10000, 10001, 1)]
        [TestCase(500, 500, 10000, 90000, 10000, 10001, 1)]
        public void CheckCondition_CorrectAmountOfConditionsGenerated(double A1, double A2, double x1, double x2, double y1, double y2, int amountOfConditions)
        {
            Track _track3 = new Track();
            Track _track4 = new Track();
            _track3.Tag = "3";
            _track1.Altitude = A1;
            _track1.PositionX = x1;
            _track1.PositionY = y1;
            _track2.Altitude = A2;
            _track2.PositionX = x2;
            _track2.PositionY = y2;
            _track3.Altitude = A2;
            _track3.PositionX = x2;
            _track3.PositionY = y2;
            //_track4.Altitude = A2;
            //_track4.PositionX = x2;
            //_track4.PositionY = y2;

            _tracks.Add(_track1);
            _tracks.Add(_track2);
            _tracks.Add(_track3);
            //_tracks.Add(_track4);

            _uut.CheckCondition(_tracks);
            Assert.That(_receivedEventArgs.ConditionsChecked.Count, Is.EqualTo(amountOfConditions));
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

            var separationToCheck = (Separation)_receivedEventArgs.ConditionsChecked.ElementAt(0);
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

        

        [TestCase(400, 500, 10000, 10001, 10000, 10001)]
        public void DataFilteredEvent_Received(double A1, double A2, double x1, double x2, double y1, double y2)
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

            _fakeFilter.FilterData(_tracks);

            var separationToCheck = (Separation)_receivedEventArgs.ConditionsChecked.ElementAt(0);
            Assert.That(separationToCheck.Tag1, Is.EqualTo("Track1"));
            Assert.That(separationToCheck.Tag2, Is.EqualTo("Track2"));
        }

    }

    [TestFixture]
    public class FilteredDataUnitTest
    {
        private DataFilteredEventArgs _receivedEventArgs;
        private AirspaceFilter _uut;

        private IStringFormatter _fakeFormatter;
        
        private int EventCount;
        private Track _track1;
        private Track _track2;
        private List<Track> _tracklist;
        
        [SetUp]
        public void SetUp()
        { 
            _receivedEventArgs = null;
            EventCount = 0;

            _track1 = new Track();
            _track2 = new Track();
            _tracklist = new List<Track>();

            //Making fakes (Stubs and mocks)
            _fakeFormatter = Substitute.For<IStringFormatter>();
            _uut = new AirspaceFilter(_fakeFormatter);
            
            //Fake Event Handler
            _uut.DataFilteredEvent += (o, args) => 
            { 
                _receivedEventArgs = args;
                EventCount++;
            };
        }

        [Test]
        public void FilteredData_HandleEvent_EventReceived()
        {
            _track1.PositionX = 50000;
            _track1.PositionY = 50000;

            _tracklist.Add(_track1);

            _uut.FilterData(_tracklist);
            Assert.That(_receivedEventArgs, Is.Not.Null);
        }

        //[Test]
        //public void FilteredData_FourTracksAddedTwoFiltered_TwoTracksInTracklist()
        //{
        //    //Test for filtering data

        //}
    }

    #region Frands' test

    [TestFixture]
    public class StringFormatterUnitTest
    {
        private StringFormatter _uut;


        [SetUp]
        public void SetUp()
        {

        }
    }

    #endregion




}
