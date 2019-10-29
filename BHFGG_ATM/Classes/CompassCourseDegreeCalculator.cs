using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    public class CompassCourseDegreeCalculator : ICompassCourseCalculator
    {
        public double CalculateCompassCourse(double oldPointX, double oldPointY, double newPointX, double newPointY)
        {
            
            double course = Math.Atan2(newPointX - oldPointX, newPointY - oldPointY);

            // convert to 0-360 compass degrees, North = 0
            course = (450 - (int)course) % 359;

            return course;
        }
        
    }
}
