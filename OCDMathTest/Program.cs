﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OCDMath.Units;

namespace OCDMathTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitDouble a = 2.0 * UnitDouble.meter;
            UnitDouble b = 3.0 * UnitDouble.meter;

            bool result = a >= b;
        }
    }
}