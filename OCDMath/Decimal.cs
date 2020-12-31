using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDMath
{
    class ExtDecimal
    {
        //Pi
        public static decimal Pi()
        {
            return 3.141592653589793238462643383279m;
        }

        //Phi (the golden ratio)
        public static decimal Phi()
        {
            return 1.618033988749894848204586834365m;
        }

        //Euler’s Number(e)
        public static decimal e()
        {
            return 2.718281828459045235360287471352m;
        }

        //Exponent
        public static decimal Exponent(decimal _number, decimal _power)
        {
            throw new NotImplementedException();
        }

        //Integer Exponent
        public static decimal IntExponent(decimal _number, int _power)
        {
            if (_power == 0) { return decimal.One; }

            decimal result = decimal.One;
            bool invert = false;
            if (_power < 0)
            {
                invert = true;
                _power = -_power;
            }
            
            while (true)
            {
                if ((_power & 1) == 1) //a check for if _power is odd (also works for _power == 1)
                {
                    result *= _number;
                }
                _power >>= 1;
                if (_power == 0) //if we've handled all of the bits for the exponent
                {
                    return invert? decimal.One / result : result; //check if we need to invert the result due to a negative power
                }
                _number *= _number;
            }  
        }

        //Natural Logarithm
        public static decimal NaturalLog(decimal _number, decimal _base)
        {
            throw new NotImplementedException();
        }

        //Logarithm
        public static decimal Log(decimal _number, decimal _base)
        {
            throw new NotImplementedException();
        }

        //Sine
        public static decimal Sine(decimal _angle_in_degrees)
        {
            throw new NotImplementedException();
        }

        //Cosine
        public static decimal Cosine(decimal _angle_in_degrees)
        {
            throw new NotImplementedException();
        }

        //Tangent
        public static decimal Tangent(decimal _angle_in_degrees)
        {
            throw new NotImplementedException();
        }

        //Cosecant
        public static decimal Cosecant(decimal _angle_in_degrees)
        {
            throw new NotImplementedException();
        }

        //Secant
        public static decimal Secant(decimal _angle_in_degrees)
        {
            throw new NotImplementedException();
        }

        //Cotangent
        public static decimal Cotangent(decimal _angle_in_degrees)
        {
            throw new NotImplementedException();
        }

        //Arc-sine
        public static decimal ArcSine(decimal _ratio)
        {
            throw new NotImplementedException();
        }

        //Arc-cosine
        public static decimal ArcCosine(decimal _ratio)
        {
            throw new NotImplementedException();
        }

        //Arc-tangent
        public static decimal ArcTangent(decimal _ratio)
        {
            throw new NotImplementedException();
        }

        //Arc-cosecant
        public static decimal ArcCosecant(decimal _ratio)
        {
            throw new NotImplementedException();
        }

        //Arc-secant
        public static decimal ArcSecant(decimal _ratio)
        {
            throw new NotImplementedException();
        }

        //Arc-cotangent
        public static decimal ArcCotangent(decimal _ratio)
        {
            throw new NotImplementedException();
        }

    }
}
