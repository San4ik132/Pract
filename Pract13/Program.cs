using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pract13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            uint i = 12;
            Console.WriteLine(DecimalToTernary(i));

        }

        static string DecimalToTernary(uint value)
        {
            if (value == 0)
            {
                return "";
            }
            else
            {
                return DecimalToTernary(value / 3) + (value % 3).ToString();
            }

        }
    }
}
