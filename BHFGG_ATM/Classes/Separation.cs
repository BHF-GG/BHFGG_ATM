using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    
    public class Separation : Condition
    {
        public string TimeStamp { get; private set; }
        public string Tag1 { get; private set; }
        public string Tag2 { get; private set; }
        

        public Separation(Track t1, Track t2, ILogCondition log)
        {
            //TimeStamp = t1.TimeStamp;
            Tag1 = t1.Tag;
            Tag2 = t2.Tag;
            LogCondition = log;
            LogOnConstruction();
        }

        ~Separation()
        {
            LogOnDestruction();
        }
    }
}
