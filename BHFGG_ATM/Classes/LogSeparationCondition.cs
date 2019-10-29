using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.Interfaces;


namespace BHFGG_ATM.Classes
{
    public class LogSeparationCondition : ILogCondition
    {
        public LogSeparationCondition()
        {
            if (!System.IO.Directory.Exists(@"C:\Logs"))
                System.IO.Directory.CreateDirectory(@"C:\Logs");
        }
        public void LogCondition(Condition c)
        {
            Separation sep = (Separation) c;
            using (System.IO.StreamWriter file = File.AppendText(@"C:\Logs\SeparationLog.txt"))
            {
                file.WriteLine($"{sep.Timestamp}: New {sep.Type} between: {sep.Tag1} and {sep.Tag2}");
            }
        }
        //string filename = String.Format("{0:yyyy-MM-dd}__{1}.txt", DateTime.Now,name);

        public void LogEndCondition(Condition c)
        {
            Separation sep = (Separation)c;
            using (System.IO.StreamWriter file = File.AppendText(@"C:\Logs\SeparationLog.txt"))
            {
                file.WriteLine($"{sep.Timestamp}: Ended {sep.Type} between: {sep.Tag1} and {sep.Tag2}");
            }
        }
    }
}
