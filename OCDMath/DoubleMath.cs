using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDMath
{
    public class DoubleMath
    {
        public static double MantissaBase10(double x)
        {
            return x / Math.Pow(10.0, ExponentBase10(x));
        }

        public static double ExponentBase10(double x)
        {
            return Math.Floor(Math.Log10(Math.Abs(x)));
        }
    }
}
