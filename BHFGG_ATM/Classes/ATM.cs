using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransponderReceiver;

namespace BHFGG_ATM.Classes
{
    public class ATM
    {
        private StringFormatter _stringFormatter;
        private AirspaceFilter _airspaceFilter;
        private ConditionChecker _conditionChecker;
        private Display _display;

        public ATM(int minimumDistance, int minimumAltitude)
        {
            var receiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            _stringFormatter = new StringFormatter(receiver,new CompassCourseDegreeCalculator(), new VelocityCalculator());
            _airspaceFilter = new AirspaceFilter(_stringFormatter);
            _conditionChecker = new ConditionChecker(minimumDistance, minimumAltitude, _airspaceFilter);
            _display = new DisplaySeparator(_airspaceFilter,_conditionChecker);
        }
    }
}
