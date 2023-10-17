using System;
using System.Numerics;

namespace MathExtensions
{
    /// <summary>
    /// The quaternion: a + bi + cj + dk; 
    /// Where (a, b, c, d) - real numbers and (i, j, k) - imagine numbers
    /// </summary>
    public struct Quaternion
    {
        public float A;
        public float B;
        public float C;
        public float D;

        public Quaternion(float A, float B, float C, float D)
        {
            this.A = A;
            this.B = B;
            this.C = C;
            this.D = D;
        }

        public static Quaternion operator +(Quaternion l, Quaternion r)
        {
            Quaternion result = new Quaternion();
            result.A = l.A + r.A;
            result.B = l.B + r.B;
            result.C = l.C + r.C;
            result.D = l.D + r.D;

            return result;
        }

        public static Quaternion operator -(Quaternion l, Quaternion r)
        {
            Quaternion result = new Quaternion();
            result.A = l.A - r.A;
            result.B = l.B - r.B;
            result.C = l.C - r.C;
            result.D = l.D - r.D;

            return result;
        }

        public static Quaternion operator *(Quaternion l, Quaternion r)
        {
            Quaternion result = new Quaternion();
            result.A = (l.A * r.A) - (l.B * r.B) - (l.C * r.C) - (l.D * r.D);
            result.B = (l.A * r.B) + (l.B * r.A) + (l.C * r.D) - (l.D * r.C);
            result.C = (l.A * r.C) + (l.C * r.A) + (l.D * r.B) - (l.B * r.D);
            result.D = (l.A * r.D) + (l.D * r.A) + (l.B * r.C) - (l.C * r.B);

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
            result.A = q.A / d;
            result.B = q.B / d;
            result.C = q.C / d;
            result.D = q.D / d;

            return result;
        }

        public static Quaternion Normalize(Quaternion q)
        {
            return q / Modulus(q);
        }

        public static Quaternion Сonjugate(Quaternion q)
        {
            return new Quaternion(q.A, -q.B, -q.C, -q.D);
        }

        public static Quaternion Reciprocal(Quaternion q)
        {
            return Сonjugate(q) / Norm(q);
        }

        public static float Norm(Quaternion q)
        {
            return q.A * q.A + q.B * q.B + q.C * q.C + q.D * q.D;
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
            return $"{Math.Round(A, 3)} + {Math.Round(B, 3)}i + {Math.Round(C, 3)}j + {Math.Round(D, 3)}k";
        }

        /// <summary>
        /// Rotates the vector along the axis by an angle in radians;
        /// When looking in the direction of the rotation axis, the vector will rotate counterclockwise by an angle in radians;
        /// </summary>
        /// <param name="vector">Vector you want to rotate</param>
        /// <param name="rotationAxis">Rotation axis</param>
        /// <param name="angle">Angle in radians</param>
        /// <returns>Roteted vector</returns>
        public static Vector3 RotateVector(Vector3 vector, Vector3 rotationAxis, float angle)
        {
            if (rotationAxis == Vector3.Zero || !VectorExtension.IsNormal(rotationAxis)
                )
            {
                throw new ArgumentException($"Rotation Axis is invalid; Rotation axis = {rotationAxis}");
            }

            if (!VectorExtension.IsNormal(vector))
            {
                throw new ArgumentException($"Vector you want to rotate is invalid; Vector = {vector}");
            }

            rotationAxis = Vector3.Normalize(rotationAxis);

            Quaternion directionQuaternion = new Quaternion(0, vector.X, vector.Y, vector.Z);

            float cos = (float)Math.Cos(angle / 2);
            float sin = (float)Math.Sin(angle / 2);
            Quaternion axisQuaternion = new Quaternion(cos, sin * rotationAxis.X, sin * rotationAxis.Y, sin * rotationAxis.Z);

            Quaternion resultQuaternion = axisQuaternion * directionQuaternion * Сonjugate(axisQuaternion);

            return new Vector3(resultQuaternion.B, resultQuaternion.C, resultQuaternion.D);
        }
    }
}
