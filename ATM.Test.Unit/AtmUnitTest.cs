using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Classes;
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
        private ConditionChecker uut;
        private Track fTrack1;
        private Track fTrack2;

        [SetUp]
        public void SetUp()
        {
            fTrack1 = new Track();
            fTrack2 = new Track();
            uut = new ConditionChecker(new AirspaceFilter(new StringFormatter()));
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
            fTrack1.PositionX = x1;
            fTrack1.PositionY = y1;
            fTrack2.PositionX = x2;
            fTrack2.PositionY = y2;

            Assert.That(uut.GetDistance(fTrack1,fTrack2).Equals(result));
        }
    }
}
