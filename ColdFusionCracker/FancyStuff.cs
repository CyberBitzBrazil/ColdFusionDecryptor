using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdFusionCracker
{
    public static class FancyStuff
    {
        public static void Header()
        {
            Console.Clear();
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("---------------  COLDFUSION PASSWORD DECRYPTER  ---------------");
            Console.WriteLine("---------------    CyberBitz Cibersegurança     ---------------");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("\r\n");
            
        }

        public static void DataHeader()
        {
            Console.WriteLine($"EMAIL:{"".PadRight(42)}PASSWORD:");
            Console.WriteLine("---------------------------------------------------------------");
        }

        public static void Footer()
        {
            Console.WriteLine("---------------------------------------------------------------");
        }
    }
}
