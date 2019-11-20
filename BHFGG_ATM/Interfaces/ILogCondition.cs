using BHFGG_ATM.Classes;

namespace BHFGG_ATM.Interfaces
{
    public interface ILogCondition
    {
        void LogCondition(Condition c);
        void LogEndCondition(Condition c);
    }
}
