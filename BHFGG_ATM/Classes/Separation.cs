using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    
    public class Separation : Condition
    {
        
        public string Tag1 { get;  set; }
        public string Tag2;
        private bool _logged;
        public Track Track1 { get; private set; }
        public Track Track2 { get; private set; }
        

        public Separation(Track t1, Track t2, int id, ILogCondition log = null, bool logCondition = false)
        {
            Track1 = t1;
            Track2 = t2;
            Type = "Separation";
            Timestamp = t1.Timestamp;
            Tag1 = t1.Tag;
            Tag2 = t2.Tag;
            Id = "S" + id.ToString();
            _logged = logCondition;
            if (log == null)
                log = new LogSeparationCondition();

            LogCondition = log;
            if (_logged)
                LogOnConstruction();
        }
        
        ~Separation()
        {
            if (_logged)
                LogOnDestruction();
        }
        
    }
}
