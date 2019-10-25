using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    public class CompassCourseDegreeCalculator : ICompassCourseCalculator
    {
        public double CalculateCompassCourse(int oldPointX, int oldPointY, int newPointX, int newPointY)
        {
            double dx = oldPointX - newPointX;
            double dy = oldPointY - newPointY;
            return Math.Atan2(dy, dx) * (180 / Math.PI);
        }

    }
}
