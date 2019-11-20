namespace BHFGG_ATM.Interfaces
{
    public interface ICompassCourseCalculator
    {
        double CalculateCompassCourse(double oldPointX, double oldPointY, double newPointX, double newPointY);
    }
}
