using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHFGG_ATM.Interfaces
{
    public interface ICompassCourseCalculator
    {
        double CalculateCompassCourse(double oldPointX, double oldPointY, double newPointX, double newPointY);
    }
}
