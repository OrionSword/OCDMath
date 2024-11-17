using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDMath.Units
{
    public struct UnitRange
    {
        private UnitDouble _min;
        public UnitDouble Min
        {
            get { return _min; }
            set
            {
                if (value <= _max)
                {
                    _min = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Attempted to set Min to a value larger than Max.");
                }
            }
        }

        private UnitDouble _max;
        public UnitDouble Max
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

        public UnitDouble Size
        {
            get { return _max - _min; }
        }

        public UnitRange(UnitDouble min, UnitDouble max)
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

        public bool InRange(UnitDouble num)
        {
            return num >= _min && num <= _max;
        }

        public bool InRangeExclusive(UnitDouble num)
        {
            return num > _min && num < _max;
        }

        public void ExpandRange(UnitDouble num)
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

        public UnitRange Copy()
        {
            return new UnitRange(_min, _max);
        }

        public static UnitRange FromUnsorted(UnitDouble a, UnitDouble b)
        {
            if (a < b)
            {
                return new UnitRange(a, b);
            }
            else
            {
                return new UnitRange(b, a);
            }
        }

        public static UnitRange ExpandRange(UnitRange range, UnitDouble num)
        {
            UnitRange newRange = range.Copy();
            newRange.ExpandRange(num);
            return newRange;
        }
    }
}
