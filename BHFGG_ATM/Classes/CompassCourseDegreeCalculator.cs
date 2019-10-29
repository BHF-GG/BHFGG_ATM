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
            double dx = oldPointX - newPointX;
            double dy = oldPointY - newPointY;
            double course = Math.Atan2(dy, dx) * (180 / Math.PI);

            if (course >= 0 && course <= 359)
            {
                return course;
            }
          
            throw new InvalidDataException("Output data not valid");


            //int getWaypointHeading(float targetLatitude, float targetLongitude)
            //{

            //    // radians
            //    float heading = atan2(targetLatitude - currentLatitude, targetLongitude - currentLongitude);

            //    // convert to degrees 0 to 180/-180
            //    heading *= (180 / 3.14159);

            //    // convert to 0-360 compass degrees, North = 0
            //    heading = (450 - (int)heading) % 360;

            //    return heading;

            //}
        }

    }
}
