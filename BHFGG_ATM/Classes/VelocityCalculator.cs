using System;
using System.IO;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    public class VelocityCalculator : IVelocityCalculator
    {
        public double CalculateCurrentVelocity(double oldPointX, double oldPointY, double newPointX, double newPointY,
            string oldTimestamp, string newTimestamp)
        {
            //Calculate distance between points
            double distance = Math.Sqrt(Math.Pow((newPointX - oldPointX), 2) + Math.Pow((newPointY - oldPointY), 2));

            //Convert strings to datetimes
            DateTime oldDate = ConvertStringTimestampToDateTime(oldTimestamp);
            DateTime newDate = ConvertStringTimestampToDateTime(newTimestamp);

            //Calculate seconds between two datetimes
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
            int millisecond = System.Convert.ToInt32(timestamp.Substring(14, 3));

            if(0 <= year
                            && 1 <= month && month <= 12 
                            && 1 <= day && day <= 31 
                            && 0 <= hour && hour <= 23 
                            && 0 <= minute && minute <= 59 
                            && 0 <= second && second <= 59 
                            && 0 <= millisecond && millisecond <= 999)
            {
                DateTime date = new DateTime(year, month, day, hour, minute, second, millisecond);
                return date;
            }
            else
            {
                throw new InvalidDataException("Time stamp not a valid date");
            }
        }

        public double CalculateSecondsBetweenDates(DateTime oldDate, DateTime newDate)
        {
            TimeSpan interval = newDate - oldDate;

            if (interval.TotalSeconds >= 0)
            {
                return interval.TotalSeconds;
            }
            else
            {
                throw new ArgumentException("oldDate is more recent than newDate");
            }
        }
    }
}
