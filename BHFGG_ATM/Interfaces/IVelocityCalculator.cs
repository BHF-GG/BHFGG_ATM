using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
