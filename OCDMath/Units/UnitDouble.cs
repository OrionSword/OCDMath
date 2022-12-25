using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDMath.Units
{
    public struct UnitDouble
    {
        //CONSTANTS
        public const string TIME_SUFFIX = "s";
        public const string LENGTH_SUFFIX = "m";
        public const string MASS_SUFFIX = "kg";
        public const string CURRENT_SUFFIX = "A";
        public const string TEMPERATURE_SUFFIX = "K";
        public const string MOLE_SUFFIX = "mol";
        public const string INTENSITY_SUFFIX = "cd";


        //FIELDS & PROPERTIES
        public double Value { get; private set; }
        public double InBaseUnits { get { return Value; } }

        public string DefaultUnitSuffix { get { return
                    ((TimeDimension != 0) ?       TIME_SUFFIX        + "^" + TimeDimension.ToString()        : "") +
                    ((TimeDimension != 0) ? "*" + LENGTH_SUFFIX      + "^" + LengthDimension.ToString()      : "") +
                    ((TimeDimension != 0) ? "*" + MASS_SUFFIX        + "^" + MassDimension.ToString()        : "") +
                    ((TimeDimension != 0) ? "*" + CURRENT_SUFFIX     + "^" + CurrentDimension.ToString()     : "") +
                    ((TimeDimension != 0) ? "*" + TEMPERATURE_SUFFIX + "^" + TemperatureDimension.ToString() : "") +
                    ((TimeDimension != 0) ? "*" + MOLE_SUFFIX        + "^" + MoleDimension.ToString()        : "") +
                    ((TimeDimension != 0) ? "*" + INTENSITY_SUFFIX   + "^" + IntensityDimension.ToString()   : "")
                    ; } }

        public int TimeDimension        { get; private set; }
        public int LengthDimension      { get; private set; }
        public int MassDimension        { get; private set; }
        public int CurrentDimension     { get; private set; }
        public int TemperatureDimension { get; private set; }
        public int MoleDimension        { get; private set; }
        public int IntensityDimension   { get; private set; }


        //CONSTRUCTORS
        public UnitDouble(double _value, int _time_exp, int _length_exp, int _mass_exp, int _current_exp, int _temperature_exp, int _mole_exp, int _intensity_exp)
        {
            Value = _value;
            TimeDimension        = _time_exp;
            LengthDimension      = _length_exp;
            MassDimension        = _mass_exp;
            CurrentDimension     = _current_exp;
            TemperatureDimension = _temperature_exp;
            MoleDimension        = _mole_exp;
            IntensityDimension   = _intensity_exp;
        }

        public UnitDouble(double _value)
        {
            Value = _value;
            TimeDimension        = 0;
            LengthDimension      = 0;
            MassDimension        = 0;
            CurrentDimension     = 0;
            TemperatureDimension = 0;
            MoleDimension        = 0;
            IntensityDimension   = 0;
        }

        public UnitDouble(double _value, UnitDouble _exp_template)
        {
            Value = _value;
            TimeDimension        = _exp_template.TimeDimension;
            LengthDimension      = _exp_template.LengthDimension;
            MassDimension        = _exp_template.MassDimension;
            CurrentDimension     = _exp_template.CurrentDimension;
            TemperatureDimension = _exp_template.TemperatureDimension;
            MoleDimension        = _exp_template.MoleDimension;
            IntensityDimension   = _exp_template.IntensityDimension;
        }


        //METHODS
        public bool HasSameUnits(UnitDouble _a)
        {
            return HasSameUnits(this, _a);
        }

        public bool IsUnitless()
        {
            return IsUnitless(this);
        }

            //the following was based on the example here:  https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1036
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return CompareTo((UnitDouble)obj);
            }
        }

        public int CompareTo(UnitDouble other)
        {
            if (!HasSameUnits(other)) throw new UnitDimensionException("Can't compare two UnitDoubles that have different dimensions.");
            if (Value == other.Value) return 0;
            if (Value > other.Value) return 1;
            return -1;
        }

        public int CompareTo(double other)
        {
            if (!this.IsUnitless()) throw new UnitDimensionException("Can't compare a Double and a UnitDouble with dimensions.");
            if (Value == other) return 0;
            if (Value > other) return 1;
            return -1;
        }

        public static int Compare(UnitDouble _a, UnitDouble _b)
        {
            if (object.ReferenceEquals(_a, _b))
            {
                return 0;
            }
            return _a.CompareTo(_b);
        }

        public static int Compare(double _a, UnitDouble _b)
        {
            if (!_b.IsUnitless()) throw new UnitDimensionException("Can't compare a Double and a UnitDouble with dimensions.");
            if (_a == _b.Value) return 0;
            if (_a > _b.Value) return 1;
            return -1;
        }

        public static int Compare(UnitDouble _a, double _b)
        {
            return _a.CompareTo(_b);
        }

        public override bool Equals(object obj)
        {
            UnitDouble _b = (UnitDouble)obj;

            if (HasSameUnits(_b))
            {
                return this.Value == _b.Value;
            }
            else
            {
                throw new UnitDimensionException("The two units being compared for equality do not have the same dimensions.");
            }
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ ((
                this.TimeDimension        ^
                this.LengthDimension      ^
                this.MassDimension        ^
                this.CurrentDimension     ^
                this.TemperatureDimension ^
                this.MoleDimension        ^
                this.IntensityDimension
                ) & 0xFF); //Overlay the 8 least-significant bits of an exclusive or of all 7 dimension integers
        }


        //OPERATORS
        // +
        public static UnitDouble operator +(UnitDouble _a, UnitDouble _b)
        {
            if (HasSameUnits(_a, _b))
            {
                return new UnitDouble(_a.Value + _b.Value, _a);
            }
            else
            {
                throw new UnitDimensionException("Two units being added do not have the same dimensions.");
            }
        }
        public static UnitDouble operator +(UnitDouble _a)
        {
            return new UnitDouble(_a.Value, _a);
        }
        public static UnitDouble operator +(double _a, UnitDouble _b)
        {
            if (_b.IsUnitless())
            {
                return new UnitDouble(_a + _b.Value);
            }
            else
            {
                throw new UnitDimensionException("Adding a dimensionless Double to a UnitDouble with dimensions is not allowed.");
            }
        }
        public static UnitDouble operator +(UnitDouble _a, double _b)
        {
            if (_a.IsUnitless())
            {
                return new UnitDouble(_a.Value + _b);
            }
            else
            {
                throw new UnitDimensionException("Adding a dimensionless Double to a UnitDouble with dimensions is not allowed.");
            }
        }

        // -
        public static UnitDouble operator -(UnitDouble _a, UnitDouble _b)
        {
            if (HasSameUnits(_a, _b))
            {
                return new UnitDouble(_a.Value - _b.Value, _a);
            }
            else
            {
                throw new UnitDimensionException("Two units being added do not have the same dimensions.");
            }
        }
        public static UnitDouble operator -(UnitDouble _a)
        {
            return new UnitDouble(-_a.Value, _a);
        }
        public static UnitDouble operator -(double _a, UnitDouble _b)
        {
            if (_b.IsUnitless())
            {
                return new UnitDouble(_a - _b.Value);
            }
            else
            {
                throw new UnitDimensionException("Subtracting a dimensionless Double from a UnitDouble with dimensions is not allowed.");
            }
        }
        public static UnitDouble operator -(UnitDouble _a, double _b)
        {
            if (_a.IsUnitless())
            {
                return new UnitDouble(_a.Value - _b);
            }
            else
            {
                throw new UnitDimensionException("Subtracting a dimensionless Double from a UnitDouble with dimensions is not allowed.");
            }
        }

        // *
        public static UnitDouble operator *(UnitDouble _a, UnitDouble _b)
        {
            return new UnitDouble(_a.Value * _b.Value,
                _a.TimeDimension        + _b.TimeDimension       ,
                _a.LengthDimension      + _b.LengthDimension     ,
                _a.MassDimension        + _b.MassDimension       ,
                _a.CurrentDimension     + _b.CurrentDimension    ,
                _a.TemperatureDimension + _b.TemperatureDimension,
                _a.MoleDimension        + _b.MoleDimension       ,
                _a.IntensityDimension   + _b.IntensityDimension
                );
        }
        public static UnitDouble operator *(double _a, UnitDouble _b)
        {
            return new UnitDouble(_a * _b.Value, _b);
        }
        public static UnitDouble operator *(UnitDouble _a, double _b)
        {
            return new UnitDouble(_a.Value * _b, _a);
        }

        // /
        public static UnitDouble operator /(UnitDouble _a, UnitDouble _b)
        {
            return new UnitDouble(_a.Value * _b.Value,
                _a.TimeDimension        - _b.TimeDimension       ,
                _a.LengthDimension      - _b.LengthDimension     ,
                _a.MassDimension        - _b.MassDimension       ,
                _a.CurrentDimension     - _b.CurrentDimension    ,
                _a.TemperatureDimension - _b.TemperatureDimension,
                _a.MoleDimension        - _b.MoleDimension       ,
                _a.IntensityDimension   - _b.IntensityDimension
                );
        }
        public static UnitDouble operator /(double _a, UnitDouble _b)
        {
            return new UnitDouble(_a / _b.Value,
                -_b.TimeDimension       ,
                -_b.LengthDimension     ,
                -_b.MassDimension       ,
                -_b.CurrentDimension    ,
                -_b.TemperatureDimension,
                -_b.MoleDimension       ,
                -_b.IntensityDimension
                );
        }
        public static UnitDouble operator /(UnitDouble _a, double _b)
        {
            return new UnitDouble(_a.Value / _b, _a);
        }

        //==
        public static bool operator ==(UnitDouble _a, UnitDouble _b)
        {
            return _a.Equals(_b);
        }

        public static bool operator !=(UnitDouble _a, UnitDouble _b)
        {
            return !_a.Equals(_b);
        }

        // >
        public static bool operator >(UnitDouble _a, UnitDouble _b)
        {
            return (Compare(_a, _b) > 0);
        }

        // >=
        public static bool operator >=(UnitDouble _a, UnitDouble _b)
        {
            return (Compare(_a, _b) >= 0);
        }

        // <
        public static bool operator <(UnitDouble _a, UnitDouble _b)
        {
            return (Compare(_a, _b) < 0);
        }

        // <=
        public static bool operator <=(UnitDouble _a, UnitDouble _b)
        {
            return (Compare(_a, _b) <= 0);
        }


        //PRESET UNITS
        //unitless
        public static UnitDouble unitless = new UnitDouble(1.0);
        public static UnitDouble radian = new UnitDouble(1.0);
        public static UnitDouble degree = new UnitDouble(System.Math.PI/180.0);
        public static UnitDouble revolution = new UnitDouble(System.Math.PI * 2);

        //time
        public static UnitDouble second = new UnitDouble(1.0, 1, 0, 0, 0, 0, 0, 0);

        //length
        public static UnitDouble meter = new UnitDouble(1.0, 0, 1, 0, 0, 0, 0, 0);

        //mass
        public static UnitDouble kilogram = new UnitDouble(1.0, 0, 0, 1, 0, 0, 0, 0);

        //current
        public static UnitDouble ampere = new UnitDouble(1.0, 0, 0, 0, 1, 0, 0, 0);
        public static UnitDouble amp = ampere;

        //temperature
        public static UnitDouble deltakelvin = new UnitDouble(1.0, 0, 0, 0, 0, 1, 0, 0);

        //mole
        public static UnitDouble mole = new UnitDouble(1.0, 0, 0, 0, 0, 0, 1, 0);

        //luminous intensity
        public static UnitDouble candela = new UnitDouble(1.0, 0, 0, 0, 0, 0, 0, 1);

        //DERIVED UNITS
        //force

        //energy

        //area

        //volume

        //EMF (voltage)

        //velocity

        //acceleration


        //STATIC METHODS
        public static bool HasSameUnits(UnitDouble _a, UnitDouble _b)
        {
            return (
                _a.TimeDimension == _b.TimeDimension &&
                _a.LengthDimension == _b.LengthDimension &&
                _a.MassDimension == _b.MassDimension &&
                _a.CurrentDimension == _b.CurrentDimension &&
                _a.TemperatureDimension == _b.TemperatureDimension &&
                _a.MoleDimension == _b.MoleDimension &&
                _a.IntensityDimension == _b.IntensityDimension
                );
        }

        public static bool IsUnitless(UnitDouble _a)
        {
            return (
                _a.TimeDimension == 0 &&
                _a.LengthDimension == 0 &&
                _a.MassDimension == 0 &&
                _a.CurrentDimension == 0 &&
                _a.TemperatureDimension == 0 &&
                _a.MoleDimension == 0 &&
                _a.IntensityDimension == 0
                );
        }


        //SUBCLASSES
        public class UnitDimensionException : Exception
        {
            public UnitDimensionException()
            {
            }

            public UnitDimensionException(string message) : base(message)
            {
            }

            public UnitDimensionException(string message, Exception inner) : base(message, inner)
            {
            }
        }


        //UTILITIES
        
    }
}
