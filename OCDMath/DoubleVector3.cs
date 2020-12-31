using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDMath
{
    public struct DoubleVector3
    {
        private const double equals_tolerance = 1e-9;
        private const double rad_to_deg = 1 / System.Math.PI * 180d;  //calculate this once to save performance doing this conversion over and over

        //PROPERTIES
        public double x;
        public double y;
        public double z;

        public double magnitude
        {
            get
            {
                return System.Math.Sqrt(x * x + y * y + z * z);
            }
        }

        public double sqrMagnitude
        {
            get => x * x + y * y + z * z;
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
                    default:
                        throw new IndexOutOfRangeException("Invalid DoubleVector3 component index!");
                }
            }
        }

        //CONSTRUCTOR
        public DoubleVector3(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        //STATIC FIELDS
        public static readonly DoubleVector3 zero =             new DoubleVector3(0d, 0d, 0d);
        public static readonly DoubleVector3 one =              new DoubleVector3(1d, 1d, 1d);
        public static readonly DoubleVector3 positiveInfinity = new DoubleVector3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        public static readonly DoubleVector3 negativeInfinity = new DoubleVector3(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

        public static readonly DoubleVector3 forward = new DoubleVector3(0d, 0d, 1d);
        public static readonly DoubleVector3 back =    new DoubleVector3(0d, 0d, -1d);
        public static readonly DoubleVector3 left =    new DoubleVector3(-1d, 0d, 0d);
        public static readonly DoubleVector3 right =   new DoubleVector3(1d, 0d, 0d);
        public static readonly DoubleVector3 up =      new DoubleVector3(0d, 1d, 0d);
        public static readonly DoubleVector3 down =    new DoubleVector3(0d, -1d, 0d);

        //METHODS
        public override bool Equals(object _other)
        {
            return (x == ((DoubleVector3)_other).x) && (y == ((DoubleVector3)_other).y) && (z == ((DoubleVector3)_other).z);
        }

        public bool Equals(DoubleVector3 _other, double _tolerance)
        {
            double x_diff = System.Math.Abs(x - _other.x);
            double y_diff = System.Math.Abs(y - _other.y);
            double z_diff = System.Math.Abs(z - _other.z);

            return (x_diff <= _tolerance) && (y_diff <= _tolerance) && (z_diff <= _tolerance);
        }

        public override int GetHashCode()
        {
            int x_int = BitConverter.ToInt32(BitConverter.GetBytes((float)x), 0); //convert x to a float (32 bits), then to a byte array, then to an int
            int y_int = BitConverter.ToInt32(BitConverter.GetBytes((float)y), 0); //convert y to a float (32 bits), then to a byte array, then to an int
            int z_int = BitConverter.ToInt32(BitConverter.GetBytes((float)z), 0); //convert z to a float (32 bits), then to a byte array, then to an int

            return ShiftAndWrap(x_int, 4) ^ ShiftAndWrap(y_int, 2) ^ z_int;  //this formula was recommended by Microsoft in their article on GetHashCode (see below for the implementation of ShiftAndWrap)
        }

        public void Set(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public string ToString(string _format)
        {
            throw new NotImplementedException();
        }

        //OPERATORS
        public static DoubleVector3 operator +(DoubleVector3 vec_a, DoubleVector3 vec_b)
        {
            return new DoubleVector3(vec_a.x + vec_b.x, vec_a.y + vec_b.y, vec_a.z + vec_b.z);
        }

        public static DoubleVector3 operator -(DoubleVector3 vec_a, DoubleVector3 vec_b)
        {
            return new DoubleVector3(vec_a.x - vec_b.x, vec_a.y - vec_b.y, vec_a.z - vec_b.z);
        }

        public static DoubleVector3 operator *(DoubleVector3 vec, double num)
        {
            return new DoubleVector3(vec.x * num, vec.y * num, vec.z * num);
        }
        public static DoubleVector3 operator *(double num, DoubleVector3 vec)
        {
            return new DoubleVector3(vec.x * num, vec.y * num, vec.z * num);
        }

        public static DoubleVector3 operator /(DoubleVector3 vec, double num)
        {
            return new DoubleVector3(vec.x / num, vec.y / num, vec.z / num);
        }

        public static bool operator ==(DoubleVector3 vec_a, DoubleVector3 vec_b)
        {
            return vec_a.Equals(vec_b, equals_tolerance);
        }

        public static bool operator !=(DoubleVector3 vec_a, DoubleVector3 vec_b)
        {
            return !vec_a.Equals(vec_b, equals_tolerance);
        }

        //STATIC METHODS
        public static double Angle(DoubleVector3 vec_a, DoubleVector3 vec_b) //Returns the angle in degrees between from and to.
        {
            double mag_a = vec_a.magnitude;
            double mag_b = vec_b.magnitude;
            if (mag_a != 0 && mag_b != 0) //check to make neither of the vectors has a magnitude of 0 (this would cause a divide by zero scenario)
            {
                return RadToDeg(System.Math.Acos(Dot(vec_a, vec_b) / (mag_a * mag_b))); //this returns degrees because Unity uses degrees for everything
            }
            return 0d; //just return 0deg if one or both of the vectors has a magnitude of 0
        }

        public static DoubleVector3 ClampMagnitude(DoubleVector3 vec, double max_length) //Returns a copy of vector with its magnitude clamped to maxLength.
        {
            double mag = vec.magnitude;
            if (mag > max_length)
            {
                return vec * (max_length / mag);
            }
            return vec;
        }

        public static DoubleVector3 Cross(DoubleVector3 vec_a, DoubleVector3 vec_b) //Cross Product of two vectors.
        {
            double c_x = vec_a.y * vec_b.z - vec_a.z * vec_b.y;
            double c_y = vec_a.z * vec_b.x - vec_a.x * vec_b.z;
            double c_z = vec_a.x * vec_b.y - vec_a.y * vec_b.x;
            return new DoubleVector3(c_x, c_y, c_z);
        }

        public static double Distance(DoubleVector3 vec_a, DoubleVector3 vec_b) //Returns the distance between a and b.
        {
            return (vec_a - vec_b).magnitude;
        }

        public static double Dot(DoubleVector3 lhs, DoubleVector3 rhs) //Dot Product of two vectors.
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        public static DoubleVector3 Lerp() //Linearly interpolates between two points.
        {
            throw new NotImplementedException();
        }

        public static DoubleVector3 LerpUnclamped() //Linearly interpolates between two vectors.
        {
            throw new NotImplementedException();
        }

        public static DoubleVector3 Max(DoubleVector3 vec_a, DoubleVector3 vec_b) //Returns a vector that is made from the largest components of two vectors.
        {
            double max_x = System.Math.Max(vec_a.x, vec_b.x);
            double max_y = System.Math.Max(vec_a.y, vec_b.y);
            double max_z = System.Math.Max(vec_a.z, vec_b.z);

            return new DoubleVector3(max_x, max_y, max_z);
        }

        public static DoubleVector3 Min(DoubleVector3 vec_a, DoubleVector3 vec_b) //Returns a vector that is made from the smallest components of two vectors.
        {
            double min_x = System.Math.Min(vec_a.x, vec_b.x);
            double min_y = System.Math.Min(vec_a.y, vec_b.y);
            double min_z = System.Math.Min(vec_a.z, vec_b.z);

            return new DoubleVector3(min_x, min_y, min_z);
        }

        public static DoubleVector3 MoveTowards() //Calculate a position between the points specified by current and target, moving no farther than the distance specified by maxDistanceDelta.
        {
            throw new NotImplementedException();
        }

        public static DoubleVector3 Normalize(DoubleVector3 vec) //Makes this vector have a magnitude of 1.
        {
            return vec.normalized;
        }

        public static void OrthoNormalize() //Makes vectors normalized and orthogonal to each other.
        {
            throw new NotImplementedException();
        }

        public static DoubleVector3 Project() //Projects a vector onto another vector.
        {
            throw new NotImplementedException();
        }

        public static DoubleVector3 ProjectOnPlane() //Projects a vector onto a plane defined by a normal orthogonal to the plane.
        {
            throw new NotImplementedException();
        }

        public static DoubleVector3 Reflect() //Reflects a vector off the plane defined by a normal.
        {
            throw new NotImplementedException();
        }

        public static DoubleVector3 RotateTowards() //Rotates a vector current towards target.
        {
            throw new NotImplementedException();
        }

        public static DoubleVector3 Scale(DoubleVector3 vec_a, DoubleVector3 vec_b) //Multiplies two vectors component-wise.
        {
            return new DoubleVector3(vec_a.x * vec_b.x, vec_a.y * vec_b.y, vec_a.z * vec_b.z);
        }

        public static double SignedAngle() //Returns the signed angle in degrees between from and to.
        {
            throw new NotImplementedException();
        }

        public static DoubleVector3 Slerp() //Spherically interpolates between two vectors.
        {
            throw new NotImplementedException();
        }

        public static DoubleVector3 SlerpUnclamped() //Spherically interpolates between two vectors.
        {
            throw new NotImplementedException();
        }

        public static DoubleVector3 SmoothDamp() //Gradually changes a vector towards a desired goal over time.
        {
            throw new NotImplementedException();
        }

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

        private static double RadToDeg(double radians)
        {
            return radians * rad_to_deg; // i.e.  * 1 / System.Math.PI * 180d;
        }

    }
}
