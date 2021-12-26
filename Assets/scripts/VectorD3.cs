using System;
using UnityEngine;

namespace Simulation
{
    [Serializable]
    public class VectorD3
    {
        // Using the pythagorean theorem
        public double sqrMagnitude => x * x + y * y + z * z;


        [SerializeField]
        public double x; 

        [SerializeField]
        public double y; 
        
        [SerializeField]
        public double z; 

        public static VectorD3 Zero => new VectorD3();

        public VectorD3(double x = 0, double y = 0, double z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        public double magnitude {
            get {
                return Math.Sqrt(x * x + y * y + z * z);
            }
        }

        public VectorD3 normalized {
            get {
                return this / magnitude;
            }
        }
        public double LengthSquared()
        {
            return x * x + y * y + z * z;
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
            return this / Math.Sqrt(x * x + y * y + z * z);
        }


        public override string ToString()
        {
            return $"VecD3 {{ X: {x}, Y: {y}, Z: {z}}}";
        }


        #region vector to vector overloads
        public static VectorD3 operator +(VectorD3 a, VectorD3 b)
        {
            return new VectorD3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static VectorD3 operator -(VectorD3 a, VectorD3 b)
        {
            return new VectorD3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        #endregion

        #region vector to scalar overloads
        public static VectorD3 operator *(double a, VectorD3 b)
        {
            return new VectorD3(a * b.x, a * b.y, a * b.z);
        }
        public static VectorD3 operator *(VectorD3 b, double a)
        {
            return new VectorD3(a * b.x, a * b.y, a * b.z);
        }
        public static VectorD3 operator /(VectorD3 a, double b)
        {
            return new VectorD3(a.x / b, a.y / b, a.z / b);
        }
        #endregion

        public static explicit operator Vector3(VectorD3 b)  // explicit byte to digit conversion operator
        {
            return new Vector3((float)b.x, (float)b.y, (float)b.z);
        }
        public double Angle => Math.Atan2(x, y);
        public static VectorD3 FromPolar(double angle, double length) {
            return new VectorD3(length * Math.Cos(angle), length * Math.Sin(angle));
        }

        // Implement the negate operator
        public static VectorD3 operator -(VectorD3 a)
        {
            return new VectorD3(-a.x, -a.y, -a.z);
        }


        public static VectorD3 Cross(VectorD3 a, VectorD3 b)
        {
            return new VectorD3(
                    (a.y * b.z) - (a.z * b.y),
                    (a.z * b.x) - (a.x * b.z),
                    (a.x * b.y) - (a.y * b.x)
                );
        }

    }
}