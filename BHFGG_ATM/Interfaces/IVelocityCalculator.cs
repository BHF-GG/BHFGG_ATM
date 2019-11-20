using System;

namespace BHFGG_ATM.Interfaces
{
    public interface IVelocityCalculator
    {
        double CalculateCurrentVelocity(double oldPointX, double oldPointY, double newPointX, double newPointY,
            string oldTimestamp, string newTimestamp);

        DateTime ConvertStringTimestampToDateTime(string timestamp);

        double CalculateSecondsBetweenDates(DateTime oldDate, DateTime newDate);
    }
}
