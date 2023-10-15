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
    }
}
