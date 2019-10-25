using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHFGG_ATM.Classes
{
    public class Track
    {
        public string Tag;
        public double PositionY;
        public double PositionX;
        public double Altitude;
        public string Timestamp;
        public double HorizontalVelocity = 0; //Default
        public double CompassCourse = 0; //Default
    }
}
