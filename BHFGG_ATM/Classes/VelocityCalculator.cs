using System;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    public class VelocityCalculator : IVelocityCalculator
    {
        public double CalculateCurrentVelocity(double oldPointX, double oldPointY, double newPointX, double newPointY,
            string oldTimestamp, string newTimestamp)
        {
            //Er ikke sikker på afstanden bliver i meter her.
            double distance = Math.Sqrt(Math.Pow((newPointX - oldPointX), 2) + Math.Pow((newPointY - oldPointY), 2));

            //Calculate seconds from timestamp  (”yyyymmddhhmmssfff”)
            DateTime oldDate = ConvertStringTimestampToDateTime(oldTimestamp);
            DateTime newDate = ConvertStringTimestampToDateTime(newTimestamp);

            //Calculate seconds between two dates
            double timeinSeconds = CalculateSecondsBetweenDates(oldDate, newDate);

            //Calculate velocity
            double velocity = distance / timeinSeconds;

            return velocity;

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

            DateTime date = new DateTime(year, month, day, hour, minute, second, milisecond);

            return date;
        }

        public double CalculateSecondsBetweenDates(DateTime oldDate, DateTime newDate)
        {
            TimeSpan interval = newDate - oldDate;
            return interval.TotalSeconds;
        }
    }
}
