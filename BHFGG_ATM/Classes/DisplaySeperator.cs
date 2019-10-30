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
        public List<Condition> ListOfConditionsToDisplay;// { get; set; }

        public override void DisplayConditions(List<Condition> conditionList)
        {
            foreach (var track in conditionList)
            {
                Separation sep = (Separation) track;
                Console.WriteLine(
                    $"--------------------------------------------------------------\n---------------------------------------------------------------------\nID: {sep.Id}\t Condition: {sep.Type}\t Timestamp: {sep.Timestamp} ----- {sep.Type} betweeen Tag1: {sep.Tag1} Tag2: {sep.Tag2}");
            }
        }

        public DisplaySeparator(IFilter filteredData, IConditionChecker conditionData)
            :  base(filteredData)
        {
            ListOfConditionsToDisplay = new List<Condition>();
            conditionData.ConditionsCheckedEvent += HandleConditionCheckedEvent;
            //ListOfConditionsToDisplay = new List<Condition>();
        }

        private void HandleConditionCheckedEvent(object sender, ConditionCheckedEventArgs e)
        {
            //Handle message/event from conditionchecked
            ListOfConditionsToDisplay = e.ConditionsChecked;
            //DisplayConditions(ListOfConditionsToDisplay);
            DisplayTracksAndConditions();
        }

        protected override void DisplayTracksAndConditions()
        {
            base.DisplayTracksAndConditions();
            DisplayConditions(ListOfConditionsToDisplay);
        }

    }
}
