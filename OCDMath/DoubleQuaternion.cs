using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDMath
{
    public struct DoubleQuaternion
    {
        private const double equals_tolerance = 1e-9;
        private const double rad_to_deg = 1 / System.Math.PI * 180d;  //calculate this once to save performance doing this conversion over and over

        //PROPERTIES
        public double x;
        public double y;
        public double z;
        public double w;

        private double magnitude
        {
            get
            {
                return System.Math.Sqrt(x * x + y * y + z * z + w * w);
            }
        }

        private double sqrMagnitude
        {
            get => x * x + y * y + z * z + w * w;
        }

        public DoubleVector3 normalized
        {
            get
            {
                double mag = this.magnitude;
                if (mag > 0)
                {
                    return this / mag;
                }
                return DoubleVector3.zero;
            }
        }

        public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    case 3:
                        return w;
                    default:
                        throw new IndexOutOfRangeException("Invalid DoubleVector3 component index!");
                }
            }
            set
            {
                switch (i)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    case 3:
                        w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid DoubleVector3 component index!");
                }
            }
        }

        //CONSTRUCTOR
        public DoubleQuaternion(double _x, double _y, double _z, double _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        //STATIC FIELDS
        public static DoubleQuaternion identity = new DoubleQuaternion(0d, 0d, 0d, 1d);

        //METHODS
        public override bool Equals(object _other)
        {
            return (x == ((DoubleQuaternion)_other).x) && (y == ((DoubleQuaternion)_other).y) && (z == ((DoubleQuaternion)_other).z) && (w == ((DoubleQuaternion)_other).w);
        }

        public bool Equals(DoubleQuaternion _other, double _tolerance)
        {
            double x_diff = System.Math.Abs(x - _other.x);
            double y_diff = System.Math.Abs(y - _other.y);
            double z_diff = System.Math.Abs(z - _other.z);
            double w_diff = System.Math.Abs(w - _other.w);

            return (x_diff <= _tolerance) && (y_diff <= _tolerance) && (z_diff <= _tolerance) && (w_diff <= _tolerance);
        }

        public override int GetHashCode()
        {
            int x_int = BitConverter.ToInt32(BitConverter.GetBytes((float)x), 0); //convert x to a float (32 bits), then to a byte array, then to an int
            int y_int = BitConverter.ToInt32(BitConverter.GetBytes((float)y), 0); //convert y to a float (32 bits), then to a byte array, then to an int
            int z_int = BitConverter.ToInt32(BitConverter.GetBytes((float)z), 0); //convert z to a float (32 bits), then to a byte array, then to an int
            int w_int = BitConverter.ToInt32(BitConverter.GetBytes((float)w), 0); //convert w to a float (32 bits), then to a byte array, then to an int

            return ShiftAndWrap(x_int, 6) ^ ShiftAndWrap(y_int, 4) ^ ShiftAndWrap(z_int, 2) ^ w_int;  //this formula was recommended by Microsoft in their article on GetHashCode (see below for the implementation of ShiftAndWrap)
        }

        public void Set(double _x, double _y, double _z, double _w) //Set x, y, z, and w components of an existing Quaternion.
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public string ToString(string _format)
        {
            throw new NotImplementedException();
        }

        public void SetFromToRotation(DoubleVector3 from_vec, DoubleVector3 to_vec) //Creates a rotation which rotates from from_vec to to_vec.
        {
            throw new NotImplementedException();
        }

        public void SetLookRotation(DoubleVector3 view) //Creates a rotation with the specified forward and upwards directions.
        {
            SetLookRotation(view, DoubleVector3.up);
        }
        public void SetLookRotation(DoubleVector3 view, DoubleVector3 up) //Creates a rotation with the specified forward and upwards directions.
        {
            throw new NotImplementedException();
        }

        public void ToAngleAxis(out double angle, out DoubleVector3 axis) //Converts a rotation to angle-axis representation (angles in degrees).
        {
            throw new NotImplementedException();
        }

        //OPERATORS
        public static DoubleVector3 operator *(DoubleQuaternion lhs, DoubleQuaternion rhs)
        {
            throw new NotImplementedException();
        }
        public static DoubleVector3 operator *(DoubleQuaternion quat, DoubleVector3 vec)
        {
            throw new NotImplementedException();
        }

        //STATIC METHODS

        //UTILITIES
        private static int ShiftAndWrap(int value, int positions)  //this method was copied from Microsoft's article on GetHashCode:  https://docs.microsoft.com/en-us/dotnet/api/system.object.gethashcode?view=netcore-3.1
        {
            positions = positions & 0x1F;

            // Save the existing bit pattern, but interpret it as an unsigned integer.
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            // Preserve the bits to be discarded.
            uint wrapped = number >> (32 - positions);
            // Shift and wrap the discarded bits.
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }
    }
}
