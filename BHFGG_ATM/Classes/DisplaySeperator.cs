using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.EventArgClasses;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM.Classes
{
    public class DisplaySeparator : Display
    {
        public List<Condition> ListOfConditionsToDisplay { get; set; }

        public override void DisplayConditions(List<Condition> conditionList)
        {
            foreach (var track in conditionList)
            {
                Separation sep = (Separation) track;
                Console.WriteLine($"Condition: {sep.Type}          Timestamp: {sep.Timestamp}");
            }
        }

        public DisplaySeparator(IFilter filteredData, IConditionChecker conditionData)
            :  base(filteredData)
        {
            conditionData.ConditionsCheckedEvent += HandleConditionCheckedEvent;
            ListOfConditionsToDisplay = new List<Condition>();
        }

        private void HandleConditionCheckedEvent(object sender, ConditionCheckedEventArgs e)
        {
            //Handle message/event from conditionchecked
            ListOfConditionsToDisplay = e.ConditionsChecked;
            DisplayConditions(ListOfConditionsToDisplay);
        }
    }
}
