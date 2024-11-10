using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDMath
{
    public struct DoubleRange
    {
        private double _min;
        public double Min {
            get { return _min;  }
            set {
                if (value <= _max)
                {
                    _min = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Attempted to set Min to a value larger than Max.");
                }
            } }

        private double _max;
        public double Max
        {
            get { return _max; }
            set
            {
                if (value >= _min)
                {
                    _max = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Attempted to set Max to a value smaller than Min.");
                }
            }
        }

        public double Size
        {
            get { return _max - _min; }
        }

        public DoubleRange(double min, double max)
        {
            if (min <= max)
            {
                _min = min;
                _max = max;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Min is greater than Max.");
            }
        }

        public bool InRange(double num)
        {
            return num >= _min && num <= _max;
        }

        public bool InRangeExclusive(double num)
        { 
            return num > _min && num < _max;
        }

        public void ExpandRange(double num)
        {
            if (InRangeExclusive(num))
            {
                throw new ArgumentOutOfRangeException("The range already includes the number provided.");
            }
            else
            {
                if (num > _max)
                {
                    _max = num;
                }
                else if (num < _min)
                {
                    _min = num;
                }
            }
        }

        public DoubleRange Copy()
        {
            return new DoubleRange(_min, _max);
        }

        public static DoubleRange FromUnsorted(double a, double b)
        {
            if (a < b)
            {
                return new DoubleRange(a, b);
            }
            else
            {
                return new DoubleRange(b, a);
            }
        }

        public static DoubleRange ExpandRange(DoubleRange range, double num)
        {
            DoubleRange newRange = range.Copy();
            newRange.ExpandRange(num);
            return newRange;
        }
    }
}
