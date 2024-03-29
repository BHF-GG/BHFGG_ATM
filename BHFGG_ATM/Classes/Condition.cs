﻿using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    public abstract class Condition
    {
        public string Type { get; protected set; }
        public string Timestamp { get; protected set; }
        public string Id { get; protected set; }
        protected ILogCondition LogCondition;
        protected virtual void LogOnConstruction()
        {
            LogCondition.LogCondition(this);
        }
        public virtual void LogOnDestruction()
        {
            LogCondition.LogEndCondition(this);
        }

    }
}
