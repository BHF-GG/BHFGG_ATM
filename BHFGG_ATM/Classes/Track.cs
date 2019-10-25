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
            DateTime oldDate = ConvertStringTimestampToDateTime(oldTimestamp);
            DateTime newDate = ConvertStringTimestampToDateTime(newTimestamp);

            //Calculate seconds between two dates
            double timeinSeconds = CalculateSecondsBetweenDates(oldDate, newDate);
            
            //Calculate velocity
            double velocity = distance / timeinSeconds;
            
            return velocity;

        }

        public double CalculateCompassCourse(int oldPointX, int oldPointY, int newPointX, int newPointY)
        {
            double dx = oldPointX - newPointX;
            double dy = oldPointY - newPointY;
            return Math.Atan2(dy, dx) * (180 / Math.PI);
        }

        public DateTime ConvertStringTimestampToDateTime(string timestamp)
        {
            int year = System.Convert.ToInt32(timestamp.Substring(0, 4));
            int month = System.Convert.ToInt32(timestamp.Substring(4, 2));
            int day = System.Convert.ToInt32(timestamp.Substring(6, 2));
            int hour = System.Convert.ToInt32(timestamp.Substring(8, 2));
            int minute = System.Convert.ToInt32(timestamp.Substring(10, 2));
            int second = System.Convert.ToInt32(timestamp.Substring(12, 2));
            int milisecond = System.Convert.ToInt32(timestamp.Substring(13, 3));

            DateTime date = new DateTime(year, month,day,hour,minute,second,milisecond);

            return date;
        }

        public double CalculateSecondsBetweenDates(DateTime oldDate, DateTime newDate)
        {
            TimeSpan interval = newDate - oldDate;
            return interval.TotalSeconds;
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
