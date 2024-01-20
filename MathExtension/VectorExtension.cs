using System;
using System.Numerics;

namespace MathExtensions
{
    public static class VectorExtension
    {
        public static bool IsNormal(this Vector3 v)
        {
            return v.X.IsNormal() && v.Y.IsNormal() && v.Z.IsNormal();
        }

        public static bool IsNormal(this Vector2 v)
        {
            return v.X.IsNormal() && v.Y.IsNormal();
        }

        public static bool IsSomeValueEqual(this Vector3 v, float value)
        {
            return v.X == value || v.Y == value || v.Z == value;
        }

        public static bool IsSomeValueEqual(this Vector2 v, float value)
        {
            return v.X == value || v.Y == value;
        }

        public static Vector3 Sign(this Vector3 v)
        {
            return new Vector3(Math.Sign(v.X), Math.Sign(v.Y), Math.Sign(v.Z));
        }

        public static Vector3 Step(this Vector3 v, Vector3 edge)
        {
            return new Vector3(MathExtension.Step(edge.X, v.X), MathExtension.Step(edge.Y, v.Y), MathExtension.Step(edge.Z, v.Z));
        }

        /// <summary>
        /// Returns angle between two vectors in radians
        /// </summary>
        /// <param name="a">Vector 1</param>
        /// <param name="b">Vector 2</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static float GetAngleBetweenVectors(Vector3 a, Vector3 b)
        {
            if (!a.IsNormal() || !b.IsNormal())
            {
                throw new ArgumentException($"Vectors aren't normal; a = {a}; b = {b}");
            }

            return (float)Math.Acos(MathExtension.Clamp(Vector3.Dot(a, b) / (a.Length() * b.Length()), -1, 1));
        }

        /// <summary>
        /// Get Rotation Axis. If rotate inputVector by an angle along the rotation axis, you get the resultVector
        /// </summary>
        /// <param name="inputVector">Vector you want to rotate</param>
        /// <param name="resultVector">Input Vector after rotate shold be</param>
        /// <param name="angle">Rotation angle</param>
        /// <returns></returns>
        public static Vector3 GetRotationAxis(Vector3 inputVector, Vector3 resultVector, float angle)
        {
            if (!inputVector.IsNormal())
            {
                throw new ArgumentException($"inputVector is invalid; inputVector = {inputVector}");
            }

            if (!resultVector.IsNormal())
            {
                throw new ArgumentException($"resultVector is invalid; resultVector = {resultVector}");
            }

            if (!angle.IsNormal())
            {
                throw new ArgumentException($"angle is invalid; angle = {angle}");
            }

            Vector3 cross = Vector3.Cross(inputVector, resultVector);

            if (!cross.IsNormal() || cross.Length() == 0)
            {
                throw new ArithmeticException($"Cross of {inputVector} and {resultVector} = {cross} it's not normal");
            }

            Vector3 rotationResult1 = Quaternion.RotateVector(inputVector, cross, angle);
            Vector3 rotationResult2 = Quaternion.RotateVector(inputVector, -cross, angle);

            if (Math.Round(GetAngleBetweenVectors(resultVector, rotationResult1)) == 0)
            {
                return cross;
            }
            else if (Math.Round(GetAngleBetweenVectors(resultVector, rotationResult2)) == 0)
            {
                return -cross;
            }
            else
            {
                throw new ArithmeticException(
                    $"Can't find rotationAxis;\n" +
                    $"resultVector = {resultVector};\n" +
                    $"rotationResult1 = {rotationResult1};\n" +
                    $"rotationResult2 = {rotationResult2};"
                    );
            }
        }
    }
}
