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
using TransponderReceiver;


namespace ATM.Test.Unit
{
    [TestFixture]
    public class AtmUnitTest
    {
        private BHFGG_ATM.Classes.ATM _uut;

        [TestCase(5000,300)]
        public void ATMConstructorTest(int d, int a)
        {
            _uut = new BHFGG_ATM.Classes.ATM(d, a);
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
            _track1.Timestamp = DateTime.Now.ToString();
            _track2.Timestamp = DateTime.Now.ToString();

            _uut = new ConditionChecker(5000,300, _fakeFilter);

            _uut.ConditionsCheckedEvent +=
                (o, args) => { _receivedEventArgs = args; };
        }

        [Test]
        public void SeparationConstructor_WithNullInConstructor()
        {
            Separation sep = new Separation(_track1,_track2,1);

            Assert.That(sep.Tag1, Is.EqualTo(_track1.Tag));
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
            var _track3 = new Track {Tag = "3"};

            _track1.Altitude = A1;
            _track1.PositionX = x1;
            _track1.PositionY = y1;

            _track2.Altitude = A2;
            _track2.PositionX = x2;
            _track2.PositionY = y2;

            _track3.Altitude = A2;
            _track3.PositionX = x2;
            _track3.PositionY = y2;

            _tracks.Add(_track1);
            _tracks.Add(_track2);
            _tracks.Add(_track3);

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

    #region Bertrams tests
    //Displaying data tests
    [TestFixture]
    public class DisplayUnitTest
    {
        //private DataFilteredEventArgs _receivedEventArgs;

        private Display _uut;

        private IFilter _fakeFilter;


        private Track _track1;
        private Track _track2;
        private Track _track3;
        private Track _track4;
        private List<Track> _tracklist;

        private List<Condition> _sepList;

        [SetUp]
        public void SetUp()
        {
            //_receivedEventArgs = null;
         
            _track1 = new Track();
            _track2 = new Track();
            _track3 = new Track();
            _track4 = new Track();
            _tracklist = new List<Track>();

            //Making fakes (Stubs and mocks)
            _fakeFilter = Substitute.For<IFilter>();

            _uut = new Display(_fakeFilter);

            _sepList = new List<Condition>();
        }

        [TestCase(50000, 50000, 40000, 40000)]
        public void HandleDataFiltered_EventReceived_DisplayAllTracks(double a1, double a2, double a3, double a4)
        {
            _track1.PositionX = a1;
            _track1.PositionY = a2;

            _track2.PositionX = a3;
            _track2.PositionY = a4;

            _tracklist.Add(_track1);
            _tracklist.Add(_track2);

            // Act: Trigger the fake object to execute event invocation
            _fakeFilter.DataFilteredEvent += Raise.EventWith(this, new DataFilteredEventArgs{DataFiltered = _tracklist});

            //Test for method that does nothing
            _uut.DisplayConditions(_sepList);

            // Assert something here or use an NSubstitute Received
            Assert.That(_uut.ListOfTracksToDisplay, Is.EqualTo(_tracklist));
        }
    }

    //Displaying data tests
    [TestFixture]
    public class DisplayConditionUnitTest
    {
        //private ConditionCheckedEventArgs _receivedEventArgs;

        private DisplaySeparator _uut;

        private ILogCondition _fakeLogCondition;
        private IConditionChecker _fakeConditionChecker;
        private IFilter _fakeFilter;

        private Separation _sep1;
        private Separation _sep2;
        private Separation _sep3;

        private Track _track1;
        private Track _track2;
        private Track _track3;
        private List<Track> _tracklist;

        private List<Condition> _sepList;

        [SetUp]
        public void SetUp()
        {
            _track1 = new Track();
            _track2 = new Track();
            _track3 = new Track();
            _tracklist = new List<Track>();

            _sepList = new List<Condition>();

            //Making fakes (Stubs and mocks)
            _fakeConditionChecker = Substitute.For<IConditionChecker>();
            _fakeFilter = Substitute.For<IFilter>();
            _fakeLogCondition = Substitute.For<ILogCondition>();

            _uut = new DisplaySeparator(_fakeFilter, _fakeConditionChecker);
        }

        [Test]
        public void DisplayCondition_ThreeConditionAdded_DisplayConditions()
        {
            _track1.PositionX = 50000;
            _track1.PositionY = 50000;

            _track2.PositionX = 50001;
            _track2.PositionY = 50001;

            _track3.PositionX = 50002;
            _track3.PositionY = 50002;

            _sep1 = new Separation(_track1, _track2, 1, _fakeLogCondition);
            _sep2 = new Separation(_track1, _track3, 2, _fakeLogCondition);
            _sep3 = new Separation(_track2, _track3, 3, _fakeLogCondition);

            _sepList.Add(_sep1);
            _sepList.Add(_sep2);
            _sepList.Add(_sep3);

            // Act: Trigger the fake object to execute event invocation
            _fakeConditionChecker.ConditionsCheckedEvent += Raise.EventWith(this, new ConditionCheckedEventArgs {ConditionsChecked = _sepList});

            // Assert something here or use an NSubstitute Received
            Assert.That(_uut.ListOfConditionsToDisplay, Is.EqualTo(_sepList));
        }

        [Test]
        public void DisplayBothNoConditionsAndConditions_ThreeTracksAdded_TwoHasCondition()
        {
            _track1.PositionX = 50000;
            _track1.PositionY = 50000;

            _track2.PositionX = 50001;
            _track2.PositionY = 50001;

            _track3.PositionX = 70002;
            _track3.PositionY = 70002;

            _sep1 = new Separation(_track1, _track2, 1, _fakeLogCondition);

            _sepList.Add(_sep1);
            _tracklist.Add(_track1);
            _tracklist.Add(_track2);
            _tracklist.Add(_track3);

            _fakeConditionChecker.ConditionsCheckedEvent += Raise.EventWith(this, new ConditionCheckedEventArgs { ConditionsChecked = _sepList });
            _fakeFilter.DataFilteredEvent += Raise.EventWith(this, new DataFilteredEventArgs { DataFiltered = _tracklist });

            Assert.That(_uut.ListOfConditionsToDisplay, Is.EqualTo(_sepList));
            Assert.That(_uut.ListOfTracksToDisplay, Is.EqualTo(_tracklist));
        }
    }

    //Datafilter tests
    [TestFixture] 
    public class FilteredDataUnitTest
    {
        private DataFilteredEventArgs _receivedEventArgs;
        private AirspaceFilter _uut;

        private IStringFormatter _fakeFormatter;
        
        private int EventCount;
        private Track _track1;
        private Track _track2;
        private Track _track3;
        private Track _track4;

        private List<Track> _tracklist;
        
        [SetUp]
        public void SetUp()
        { 
            _receivedEventArgs = null;
            EventCount = 0;

            _track1 = new Track();
            _track2 = new Track();
            _track3 = new Track();
            _track4 = new Track();
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

        [Test]
        public void HandleDataFilteredEvent_EventReceived()
        {
            _track1.PositionX = 50000;
            _track1.PositionY = 50000;

            _tracklist.Add(_track1);

            // Act: Trigger the fake object to execute event invocation
            _fakeFormatter.DataFormattedEvent += Raise.EventWith(this, new DataFormattedEventArgs{DataFormatted = _tracklist});

            // Assert something here or use an NSubstitute Received
            Assert.That(_uut.CurrentListOfTracks, Is.EqualTo(_tracklist));
        }

        [Test]
        public void FilteredData_FourTracksAdded_TwoTracksFiltered()
        {
            _track1.PositionX = 50000;
            _track1.PositionY = 50000;
            _track1.Altitude = 1000;

            _track2.PositionX = 50000;
            _track2.PositionY = 50000;
            _track2.Altitude = 1000;

            _track3.PositionX = 100;
            _track3.PositionY = 9000;
            _track3.Altitude = 100;

            _track4.PositionX = 100;
            _track4.PositionY = 9000;
            _track4.Altitude = 100;

            _tracklist.Add(_track1);
            _tracklist.Add(_track2);
            _tracklist.Add(_track3);
            _tracklist.Add(_track4);


            _uut.FilterData(_tracklist);

            Assert.That(_uut.CurrentListOfTracks.Count, Is.EqualTo(2));
        }
    }
    #endregion

    #region Frands' test

    [TestFixture]
    public class StringFormatterUnitTest
    {
        private ITransponderReceiver _fakeTransponderReceiver;
        private ICompassCourseCalculator _fakeCompassCourseCalculator;
        private IVelocityCalculator _fakeVelocityCalculator;

        private StringFormatter _uut;


        [SetUp]
        public void SetUp()
        {
            // Make a fake Transponder Data Receiver
            _fakeTransponderReceiver = Substitute.For<ITransponderReceiver>();
            _fakeCompassCourseCalculator = Substitute.For<ICompassCourseCalculator>();
            _fakeVelocityCalculator = Substitute.For<IVelocityCalculator>();

            // Inject the fake StringFormatter
            _uut = new StringFormatter(_fakeTransponderReceiver,_fakeCompassCourseCalculator,_fakeVelocityCalculator);
        }

        [Test]
        public void HandleTransponderDataEvent_EventReceived_OK()
        {
            // Setup test data
            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            testData.Add("BCD123;10005;85890;12000;20151006213456789");
            testData.Add("XYZ987;25059;75654;4000;20151006213456789");

            // Act: Trigger the fake object to execute event invocation
            _fakeTransponderReceiver.TransponderDataReady
                += Raise.EventWith(this, new RawTransponderDataEventArgs(testData));

            // Assert something here or use an NSubstitute Received
            Assert.That(_uut.CurrentTransponderData,Is.EqualTo(testData));
        }

        //public void FormatData_EventRaised_OK()
        //{
        //    //Use setup from different scenario
        //    HandleTransponderDataEvent_EventReceived_OK();

        //    // Act: 
        //    // Assert something here or use an NSubstitute Received
        //    Assert.That(_uut.CurrentTransponderData, Is.EqualTo(testData));
        //}


    }

    #endregion




}
