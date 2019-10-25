using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHFGG_ATM.Classes
{
    public class Track
    {
        //Disse to metoder skal flyttes til en anden klasse! Ellers sendes de med rundt
        public double CalculateCurrentVelocity(int oldPointX, int oldPointY, int newPointX, int newPointY, string oldTimestamp, string newTimestamp)
        {
            //Er ikke sikker på afstanden bliver i meter her.
            double distance =  Math.Sqrt(Math.Pow((newPointX - oldPointX), 2) + Math.Pow((newPointY - oldPointY), 2));

            
            //Calculate seconds from timestamp  (”yyyymmddhhmmssfff”)
            int oldTimestampInt = System.Convert.ToInt32(oldTimestamp);
            int newTimestampInt = System.Convert.ToInt32(newTimestamp);




            double timeinSeconds = 0;
            
            double velocity = distance / timeinSeconds;
            
            return velocity;

        }

        public double CalculateCompassCourse(int oldPointX, int oldPointY, int newPointX, int newPointY)
        {
            double dx = oldPointX - newPointX;
            double dy = oldPointY - newPointY;
            return Math.Atan2(dy, dx) * (180 / Math.PI);
        }

        public string Tag;
        public double PositionY;
        public double PositionX;
        public double Altitude;
        public double HorizontalVelocity;
        public double CompassCourse;
        public string Timestamp;

    }
}
