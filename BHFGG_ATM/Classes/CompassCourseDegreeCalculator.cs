using BHFGG_ATM.Interfaces;
using System;

namespace BHFGG_ATM.Classes
{
    public class CompassCourseDegreeCalculator : ICompassCourseCalculator
    {
        public double CalculateCompassCourse(double oldPointX, double oldPointY, double newPointX, double newPointY)
        {
            double radians2degree = 180 / Math.PI;

            double dy = newPointY - oldPointY;
            double dx = newPointX - oldPointX;

            double course = Math.Atan2(dy, dx) * radians2degree;

            if (course < 0)
            {
                course += 360;
            }

            return course;
        }
    }
}
