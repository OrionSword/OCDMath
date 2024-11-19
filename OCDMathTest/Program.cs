using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OCDMath.Units;
using OCDMath;

namespace OCDMathTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitDouble a = 2.0 * UnitDouble.meter;
            UnitDouble b = 3.0 * UnitDouble.meter;

            Console.WriteLine(a >= b);
            Console.WriteLine((a + b).ToString());
            Console.WriteLine((a * b).ToString());

            DoubleRange doubleRange = new(2.0, 3.5);
            
            Console.WriteLine(doubleRange.Size.ToString());


            //UnitRange
            Console.WriteLine("UnitRange");
            UnitRange unitRange = new(2.0 * UnitDouble.meter, 3.5 * UnitDouble.meter);

            Console.WriteLine(unitRange);
            Console.WriteLine(unitRange.Size.ToString());
            unitRange.ExpandRange(5.0 * UnitDouble.meter);
            Console.WriteLine(unitRange);
            unitRange.ExpandRange(1.0 * UnitDouble.meter);
            Console.WriteLine(unitRange);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
