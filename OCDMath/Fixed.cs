using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDMath
{
    public struct Fixed// : IComparable
    {
        public int sign { get => getSign(); }
        public bool positive { get; private set; }
        public int bytes { get ; private set; }
        public int intBytes { get => bytes - shiftBytes; } //this can be negative
        public int fractionBytes { get => shiftBytes; }
        public int shiftBytes { get; private set; }
        private byte[] number; //index 0 is the LEAST SIGNIFICANT digit (the digit that is furthest to the right)

        public Fixed(uint _bytes, uint _shiftbytes)
        {
            //shiftBytes=0 means the Fixed is effectively an integer
            //shiftBytes=1 means there is 1 byte (8 bits) of fractional precision
            number = new byte[_bytes];
            bytes = (int)_bytes;
            shiftBytes = (int)_shiftbytes;
        }

        public static Fixed operator+ (Fixed a, Fixed b)
        {

        }

        private int getSign()
        {
            if (isZero())
            {
                return 0;
            }
            else
            {
                return positive ? 1 : -1;
            }
        }

        public bool isZero()
        {
            for (int i = 0; i < bytes; i++)
            {
                if (number[i]!=0) { return false; }
            }
            return true;
        }

    }
}
