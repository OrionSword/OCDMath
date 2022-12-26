using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDMath.Units
{
    public struct UnitDouble :
        IComparable<UnitDouble>, IComparable<double>, IEquatable<UnitDouble>, IEquatable<double>
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

        public string DefaultUnitSuffix { get {
                string value =
                    ((TimeDimension != 0)        ?       TIME_SUFFIX        + "^" + TimeDimension.ToString()        : "") +
                    ((LengthDimension != 0)      ? "*" + LENGTH_SUFFIX      + "^" + LengthDimension.ToString()      : "") +
                    ((MassDimension != 0)        ? "*" + MASS_SUFFIX        + "^" + MassDimension.ToString()        : "") +
                    ((CurrentDimension != 0)     ? "*" + CURRENT_SUFFIX     + "^" + CurrentDimension.ToString()     : "") +
                    ((TemperatureDimension != 0) ? "*" + TEMPERATURE_SUFFIX + "^" + TemperatureDimension.ToString() : "") +
                    ((MoleDimension != 0)        ? "*" + MOLE_SUFFIX        + "^" + MoleDimension.ToString()        : "") +
                    ((IntensityDimension != 0)   ? "*" + INTENSITY_SUFFIX   + "^" + IntensityDimension.ToString()   : "");
                return (value.Substring(0, 1) == "*") ? value.Substring(1) : value;
            } }

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

        public override string ToString()
        {
            return Value.ToString() + DefaultUnitSuffix;
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
            if (!HasSameUnits(other)) throw new UnitDimensionException("The two units being compared do not have the same dimensions.");
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

        public override bool Equals(object other)
        {
            return Equals((UnitDouble)other);
        }
        public bool Equals(UnitDouble other)
        {
            if (HasSameUnits(other))
            {
                return this.Value == other.Value;
            }
            else
            {
                throw new UnitDimensionException("The two units being compared for equality do not have the same dimensions.");
            }
        }
        public bool Equals(double other)
        {
            if (IsUnitless())
            {
                return this.Value == other;
            }
            else
            {
                throw new UnitDimensionException("Can't compare a Double and a UnitDouble with dimensions.");
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

        public static bool Equal(UnitDouble _a, UnitDouble _b)
        {
            if (_a.HasSameUnits(_b))
            {
                return _a.Value == _b.Value;
            }
            else
            {
                throw new UnitDimensionException("The two units being compared do not have the same dimensions.");
            }
        }
        public static bool Equal(double _a, UnitDouble _b)
        {
            if (_b.IsUnitless())
            {
                return _a == _b.Value;
            }
            else
            {
                throw new UnitDimensionException("Can't compare a Double and a UnitDouble with dimensions.");
            }
        }
        public static bool Equal(UnitDouble _a, double _b)
        {
            if (_a.IsUnitless())
            {
                return _a.Value == _b;
            }
            else
            {
                throw new UnitDimensionException("Can't compare a Double and a UnitDouble with dimensions.");
            }
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

        // ==
        public static bool operator ==(UnitDouble _a, UnitDouble _b)
        {
            return Equals(_a, _b);
        }
        public static bool operator ==(double _a, UnitDouble _b)
        {
            return Equals(_a, _b);
        }
        public static bool operator ==(UnitDouble _a, double _b)
        {
            return Equals(_a, _b);
        }

        // !=
        public static bool operator !=(UnitDouble _a, UnitDouble _b)
        {
            return !Equals(_a, _b);
        }
        public static bool operator !=(double _a, UnitDouble _b)
        {
            return !Equals(_a, _b);
        }
        public static bool operator !=(UnitDouble _a, double _b)
        {
            return !Equals(_a, _b);
        }

        // >
        public static bool operator >(UnitDouble _a, UnitDouble _b)
        {
            return (Compare(_a, _b) > 0);
        }
        public static bool operator >(double _a, UnitDouble _b)
        {
            return (Compare(_a, _b) > 0);
        }
        public static bool operator >(UnitDouble _a, double _b)
        {
            return (Compare(_a, _b) > 0);
        }

        // >=
        public static bool operator >=(UnitDouble _a, UnitDouble _b)
        {
            return (Compare(_a, _b) >= 0);
        }
        public static bool operator >=(double _a, UnitDouble _b)
        {
            return (Compare(_a, _b) >= 0);
        }
        public static bool operator >=(UnitDouble _a, double _b)
        {
            return (Compare(_a, _b) >= 0);
        }

        // <
        public static bool operator <(UnitDouble _a, UnitDouble _b)
        {
            return (Compare(_a, _b) < 0);
        }
        public static bool operator <(double _a, UnitDouble _b)
        {
            return (Compare(_a, _b) < 0);
        }
        public static bool operator <(UnitDouble _a, double _b)
        {
            return (Compare(_a, _b) < 0);
        }

        // <=
        public static bool operator <=(UnitDouble _a, UnitDouble _b)
        {
            return (Compare(_a, _b) <= 0);
        }
        public static bool operator <=(double _a, UnitDouble _b)
        {
            return (Compare(_a, _b) <= 0);
        }
        public static bool operator <=(UnitDouble _a, double _b)
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
        public static UnitDouble millisecond = new UnitDouble(0.001, 1, 0, 0, 0, 0, 0, 0);
        public static UnitDouble microsecond = new UnitDouble(0.000001, 1, 0, 0, 0, 0, 0, 0);
        public static UnitDouble minute = new UnitDouble(1/60.0, 1, 0, 0, 0, 0, 0, 0);
        public static UnitDouble hour   = new UnitDouble(1/3600.0, 1, 0, 0, 0, 0, 0, 0);

        //length
        public static UnitDouble meter = new UnitDouble(1.0, 0, 1, 0, 0, 0, 0, 0);
        public static UnitDouble kilometer = new UnitDouble(1000.0, 0, 1, 0, 0, 0, 0, 0);
        public static UnitDouble centimeter = new UnitDouble(0.01, 0, 1, 0, 0, 0, 0, 0);
        public static UnitDouble millimeter = new UnitDouble(0.001, 0, 1, 0, 0, 0, 0, 0);
        public static UnitDouble micrometer = new UnitDouble(0.000001, 0, 1, 0, 0, 0, 0, 0);
        public static UnitDouble inch = new UnitDouble(0.0254, 0, 1, 0, 0, 0, 0, 0);
        public static UnitDouble foot = new UnitDouble(0.3048, 0, 1, 0, 0, 0, 0, 0);
        public static UnitDouble mile = new UnitDouble(1609.34, 0, 1, 0, 0, 0, 0, 0);

        //mass
        public static UnitDouble kilogram = new UnitDouble(1.0, 0, 0, 1, 0, 0, 0, 0);
        public static UnitDouble gram = new UnitDouble(0.001, 0, 0, 1, 0, 0, 0, 0);
        public static UnitDouble milligram = new UnitDouble(0.000001, 0, 0, 1, 0, 0, 0, 0);
        public static UnitDouble tonne = new UnitDouble(1000.0, 0, 0, 1, 0, 0, 0, 0);
        public static UnitDouble poundMass = new UnitDouble(0.453592, 0, 0, 1, 0, 0, 0, 0);
        public static UnitDouble ounceMass = new UnitDouble(0.0283495, 0, 0, 1, 0, 0, 0, 0);
        public static UnitDouble tonMass = new UnitDouble(907.1847, 0, 0, 1, 0, 0, 0, 0);

        //current
        public static UnitDouble ampere = new UnitDouble(1.0, 0, 0, 0, 1, 0, 0, 0);
        public static UnitDouble amp = ampere;
        public static UnitDouble milliamp = new UnitDouble(0.001, 0, 0, 0, 1, 0, 0, 0);

        //temperature
        public static UnitDouble deltaKelvin = new UnitDouble(1.0, 0, 0, 0, 0, 1, 0, 0);
        public static UnitDouble deltaCelsius = new UnitDouble(1.0, 0, 0, 0, 0, 1, 0, 0);
        public static UnitDouble deltaFahrenheit = new UnitDouble(5.0/9.0, 0, 0, 0, 0, 1, 0, 0);
        public static UnitDouble deltaRankine = new UnitDouble(5.0/9.0, 0, 0, 0, 0, 1, 0, 0);

        //mole
        public static UnitDouble mole = new UnitDouble(1.0, 0, 0, 0, 0, 0, 1, 0);

        //luminous intensity
        public static UnitDouble candela = new UnitDouble(1.0, 0, 0, 0, 0, 0, 0, 1);


        //DERIVED UNITS
        //frequency
        public static UnitDouble hertz = new UnitDouble(1.0, -1, 0, 0, 0, 0, 0, 0);
        public static UnitDouble kilohertz = new UnitDouble(1000.0, -1, 0, 0, 0, 0, 0, 0);
        public static UnitDouble megahertz = new UnitDouble(1000000.0, -1, 0, 0, 0, 0, 0, 0);
        public static UnitDouble gigahertz = new UnitDouble(1000000000.0, -1, 0, 0, 0, 0, 0, 0);

        //force
        public static UnitDouble newton = new UnitDouble(1.0, -2, 1, 1, 0, 0, 0, 0);
        public static UnitDouble millinewton = new UnitDouble(0.001, -2, 1, 1, 0, 0, 0, 0);
        public static UnitDouble kilonewton = new UnitDouble(1000.0, -2, 1, 1, 0, 0, 0, 0);
        public static UnitDouble gramForce = new UnitDouble(0.00980665, -2, 1, 1, 0, 0, 0, 0);
        public static UnitDouble kilogramForce = new UnitDouble(9.80665, -2, 1, 1, 0, 0, 0, 0);
        public static UnitDouble tonneForce = new UnitDouble(9806.65, -2, 1, 1, 0, 0, 0, 0);
        public static UnitDouble ounceForce = new UnitDouble(0.27801, -2, 1, 1, 0, 0, 0, 0);
        public static UnitDouble poundForce = new UnitDouble(4.44822, -2, 1, 1, 0, 0, 0, 0);
        public static UnitDouble tonForce = new UnitDouble(8896.443, -2, 1, 1, 0, 0, 0, 0);

        //pressure & stress
        public static UnitDouble pascal = new UnitDouble(1.0, -2, -1, 1, 0, 0, 0, 0);
        public static UnitDouble kilopascal = new UnitDouble(1000.0, -2, -1, 1, 0, 0, 0, 0);
        public static UnitDouble megapascal = new UnitDouble(1000000.0, -2, -1, 1, 0, 0, 0, 0);
        public static UnitDouble gigapascal = new UnitDouble(1000000000.0, -2, -1, 1, 0, 0, 0, 0);
        public static UnitDouble bar = new UnitDouble(100000.0, -2, -1, 1, 0, 0, 0, 0);
        public static UnitDouble atmosphere = new UnitDouble(101325.0, -2, -1, 1, 0, 0, 0, 0);
        public static UnitDouble poundPerSquareInch = new UnitDouble(6894.76, -2, -1, 1, 0, 0, 0, 0);

        //energy
        public static UnitDouble joule = new UnitDouble(1.0, -2, 2, 1, 0, 0, 0, 0);
        public static UnitDouble millijoule = new UnitDouble(0.001, -2, 2, 1, 0, 0, 0, 0);
        public static UnitDouble kilojoule = new UnitDouble(1000.0, -2, 2, 1, 0, 0, 0, 0);
        public static UnitDouble megajoule = new UnitDouble(1000000.0, -2, 2, 1, 0, 0, 0, 0);
        public static UnitDouble gigajoule = new UnitDouble(1000000000.0, -2, 2, 1, 0, 0, 0, 0);
        public static UnitDouble britishThermalUnit = new UnitDouble(1055.06, -2, 2, 1, 0, 0, 0, 0);

        //power
        public static UnitDouble watt = new UnitDouble(1.0, -3, 2, 1, 0, 0, 0, 0);
        public static UnitDouble milliwatt = new UnitDouble(0.001, -3, 2, 1, 0, 0, 0, 0);
        public static UnitDouble kilowatt = new UnitDouble(1000.0, -3, 2, 1, 0, 0, 0, 0);
        public static UnitDouble megawatt = new UnitDouble(1000000.0, -3, 2, 1, 0, 0, 0, 0);
        public static UnitDouble gigawatt = new UnitDouble(1000000000.0, -3, 2, 1, 0, 0, 0, 0);
        public static UnitDouble horsepower = new UnitDouble(745.7, -3, 2, 1, 0, 0, 0, 0);

        //area
        public static UnitDouble meterSquared = new UnitDouble(1.0, 0, 2, 0, 0, 0, 0, 0);
        public static UnitDouble millimeterSquared = new UnitDouble(0.000001, 0, 2, 0, 0, 0, 0, 0);
        public static UnitDouble centimeterSquared = new UnitDouble(0.0001, 0, 2, 0, 0, 0, 0, 0);
        public static UnitDouble kilometerSquared = new UnitDouble(1000000.0, 0, 2, 0, 0, 0, 0, 0);
        public static UnitDouble inchSquared = new UnitDouble(0.00064516, 0, 2, 0, 0, 0, 0, 0);
        public static UnitDouble footSquared = new UnitDouble(0.092903, 0, 2, 0, 0, 0, 0, 0);
        public static UnitDouble acre = new UnitDouble(4046.86, 0, 2, 0, 0, 0, 0, 0);
        public static UnitDouble hectare = new UnitDouble(10000.0, 0, 2, 0, 0, 0, 0, 0);
        public static UnitDouble mileSquared = new UnitDouble(258998.110336, 0, 2, 0, 0, 0, 0, 0);

        //volume
        public static UnitDouble meterCubed = new UnitDouble(1.0, 0, 3, 0, 0, 0, 0, 0);
        public static UnitDouble millimeterCubed = new UnitDouble(0.000000001, 0, 3, 0, 0, 0, 0, 0);
        public static UnitDouble centimeterCubed = new UnitDouble(0.000001, 0, 3, 0, 0, 0, 0, 0);
        public static UnitDouble kilometerCubed = new UnitDouble(1000000.0, 0, 3, 0, 0, 0, 0, 0);
        public static UnitDouble liter = new UnitDouble(0.001, 0, 3, 0, 0, 0, 0, 0);
        public static UnitDouble milliliter = new UnitDouble(0.000001, 0, 3, 0, 0, 0, 0, 0);
        public static UnitDouble inchCubed = new UnitDouble(0.0000163871, 0, 3, 0, 0, 0, 0, 0);
        public static UnitDouble footCubed = new UnitDouble(0.0283168, 0, 3, 0, 0, 0, 0, 0);
        public static UnitDouble gallon = new UnitDouble(0.00378541, 0, 3, 0, 0, 0, 0, 0);
        public static UnitDouble fluidOunce = new UnitDouble(0.0000295735, 0, 3, 0, 0, 0, 0, 0);


        //charge
        public static UnitDouble coulomb = new UnitDouble(1.0, 1, 0, 0, 1, 0, 0, 0);

        //EMF (voltage)
        public static UnitDouble volt = new UnitDouble(1.0, -3, 2, 1, -1, 0, 0, 0);
        public static UnitDouble millivolt = new UnitDouble(0.001, -3, 2, 1, -1, 0, 0, 0);
        public static UnitDouble kilovolt = new UnitDouble(1000.0, -3, 2, 1, -1, 0, 0, 0);

        //velocity
        public static UnitDouble meterPerSecond = new UnitDouble(1.0, -1, 1, 0, 0, 0, 0, 0);
        public static UnitDouble millimeterPerSecond = new UnitDouble(0.001, -1, 1, 0, 0, 0, 0, 0);
        public static UnitDouble kilometerPerSecond = new UnitDouble(1000.0, -1, 1, 0, 0, 0, 0, 0);
        public static UnitDouble inchPerSecond = new UnitDouble(0.0254, -1, 1, 0, 0, 0, 0, 0);
        public static UnitDouble footPerSecond = new UnitDouble(0.3048, -1, 1, 0, 0, 0, 0, 0);
        public static UnitDouble kilometerPerHour = new UnitDouble(1/3.6, -1, 1, 0, 0, 0, 0, 0);
        public static UnitDouble milePerHour = new UnitDouble(0.44704, -1, 1, 0, 0, 0, 0, 0);

        //acceleration
        public static UnitDouble meterPerSecondSquared = new UnitDouble(1.0, -2, 1, 0, 0, 0, 0, 0);
        public static UnitDouble inchPerSecondSquared = new UnitDouble(0.0254, -2, 1, 0, 0, 0, 0, 0);
        public static UnitDouble footPerSecondSquared = new UnitDouble(0.3048, -2, 1, 0, 0, 0, 0, 0);
        public static UnitDouble gAcceleration = new UnitDouble(9.80665, -2, 1, 0, 0, 0, 0, 0);


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
    }
}
