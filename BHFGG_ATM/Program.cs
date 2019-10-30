using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BHFGG_ATM.Classes;
using BHFGG_ATM.Interfaces;

namespace BHFGG_ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            ATM atm1 = new ATM(5000,300);
            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
