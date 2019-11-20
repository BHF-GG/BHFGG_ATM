using BHFGG_ATM.Classes;
using System.Threading;

namespace BHFGG_ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            ATM atm1 = new ATM(5000, 300);
            while (true)
            {
                Thread.Sleep(10);
            }
        }
    }
}
