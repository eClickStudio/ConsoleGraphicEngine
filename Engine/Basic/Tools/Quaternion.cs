using System;
using System.Numerics;

namespace ConsoleGraphicEngine.Engine.Basic.Tools
{
    /// <summary>
    /// The quaternion: a + bi + cj + dk; 
    /// Where (a, b, c, d) - real numbers and (i, j, k) - imagine numbers
    /// </summary>
    internal struct Quaternion
    {
        public float a;
        public float b;
        public float c;
        public float d;

        public Quaternion(float a, float b, float c, float d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public static Quaternion operator +(Quaternion l, Quaternion r)
        {
            Quaternion result = new Quaternion();
            result.a = l.a + r.a;
            result.b = l.b + r.b;
            result.c = l.c + r.c;
            result.d = l.d + r.d;

            return result;
        }

        public static Quaternion operator -(Quaternion l, Quaternion r)
        {
            Quaternion result = new Quaternion();
            result.a = l.a - r.a;
            result.b = l.b - r.b;
            result.c = l.c - r.c;
            result.d = l.d - r.d;

            return result;
        }

        public static Quaternion operator *(Quaternion l, Quaternion r)
        {
            Quaternion result = new Quaternion();
            result.a = (l.a * r.a) - (l.b * r.b) - (l.c * r.c) - (l.d * r.d);
            result.b = (l.a * r.b) + (l.b * r.a) + (l.c * r.d) - (l.d * r.c);
            result.c = (l.a * r.c) + (l.c * r.a) + (l.d * r.b) - (l.b * r.d);
            result.d = (l.a * r.d) + (l.d * r.a) + (l.b * r.c) - (l.c * r.b);

            return result;
        }

        public static Quaternion DivideLeft(Quaternion l, Quaternion r)
        {
            return Reciprocal(l) * r;
        }

        public static Quaternion DivideRight(Quaternion l, Quaternion r)
        {
            return l * Reciprocal(r);
        }

        public static Quaternion operator /(Quaternion q, float d)
        {
            Quaternion result = new Quaternion();
            result.a = q.a / d;
            result.b = q.b / d;
            result.c = q.c / d;
            result.d = q.d / d;

            return result;
        }

        public static Quaternion Normalize(Quaternion q)
        {
            return q / Modulus(q);
        }

        public static Quaternion Сonjugate(Quaternion q)
        {
            return new Quaternion(q.a, -q.b, -q.c, -q.d);
        }

        public static Quaternion Reciprocal(Quaternion q)
        {
            return Сonjugate(q) / Norm(q);
        }

        public static float Norm(Quaternion q)
        {
            return q.a * q.a + q.b * q.b + q.c * q.c + q.d * q.d;
        }

        public static float Modulus(Quaternion q)
        {
            return (float)Math.Sqrt(Norm(q));
        }

        public static float Distance(Quaternion q1, Quaternion q2)
        {
            return Modulus(q1 - q2);
        }

        public override string ToString()
        {
            return $"{Math.Round(a, 3)} + {Math.Round(b, 3)}i + {Math.Round(c, 3)}j + {Math.Round(d, 3)}k";
        }

        /// <summary>
        /// Rotates the vector
        /// </summary>
        /// <param name="vector">Vector you want to rotate</param>
        /// <param name="rotationAxis">Rotation axis</param>
        /// <param name="angle">Angle in radians</param>
        /// <returns>Roteted vector</returns>
        /// <exception cref="ArgumentException">Rotation Axis cannot be zero (0; 0)</exception>
        public static Vector3 RotateVector(Vector3 vector, Vector3 rotationAxis, float angle)
        {
            if (rotationAxis == Vector3.Zero)
            {
                throw new ArgumentException("Rotation Axis cannot be zero (0; 0)!");
            }

            rotationAxis = Vector3.Normalize(rotationAxis);

            Quaternion directionQuaternion = new Quaternion(0, vector.X, vector.Y, vector.Z);

            float cos = (float)Math.Cos(angle / 2);
            float sin = (float)Math.Sin(angle / 2);
            Quaternion axisQuaternion = new Quaternion(cos, sin * rotationAxis.X, sin * rotationAxis.Y, sin * rotationAxis.Z);

            Quaternion resultQuaternion = axisQuaternion * directionQuaternion * Сonjugate(axisQuaternion);

            return new Vector3(resultQuaternion.b, resultQuaternion.c, resultQuaternion.d);
        }
    }
}
