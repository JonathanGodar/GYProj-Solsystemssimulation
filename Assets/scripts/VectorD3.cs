using System;
using UnityEngine;

namespace Simulation
{
    public class VectorD3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public static VectorD3 Zero => new VectorD3();

        public VectorD3(double x = 0, double y = 0, double z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }


        public double LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        /**
         * Resource intensive, use carefully
         */
        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        public static double DistanceSquared(VectorD3 a, VectorD3 b)
        {
            return (a - b).LengthSquared();
        }

        /**
         * Resource intensive, use carefully
         */
        public VectorD3 Normalized()
        {
            return this / Math.Sqrt(X * X + Y * Y + Z * Z);
        }


        public override string ToString()
        {
            return $"VecD3 {{ X: {X}, Y: {Y}, Z: {Z}}}";
        }


        #region vector to vector overloads
        public static VectorD3 operator +(VectorD3 a, VectorD3 b)
        {
            return new VectorD3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static VectorD3 operator -(VectorD3 a, VectorD3 b)
        {
            return new VectorD3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        #endregion

        #region vector to scalar overloads
        public static VectorD3 operator *(double a, VectorD3 b)
        {
            return new VectorD3(a * b.X, a * b.Y, a * b.Z);
        }
        public static VectorD3 operator *(VectorD3 b, double a)
        {
            return new VectorD3(a * b.X, a * b.Y, a * b.Z);
        }
        public static VectorD3 operator /(VectorD3 a, double b)
        {
            return new VectorD3(a.X / b, a.Y / b, a.Z / b);
        }
        #endregion

        public static explicit operator Vector3(VectorD3 b)  // explicit byte to digit conversion operator
        {
            return new Vector3((float)b.X, (float)b.Y, (float)b.Z);
        }
        public double Angle => Math.Atan2(X, Y);
        public static VectorD3 FromPolar(double angle, double length) {
            return new VectorD3(length * Math.Cos(angle), length * Math.Sin(angle));
        }

        public static VectorD3 Cross(VectorD3 a, VectorD3 b)
        {
            return new VectorD3(
                    (a.Y * b.Z) - (a.Z * b.Y),
                    (a.Z * b.X) - (a.X * b.Z),
                    (a.X * b.Y) - (a.Y * b.X)
                );
        }

    }
}