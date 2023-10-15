using System;

namespace MathExtensions
{
    public static class MathExtension
    {
        public static float Clamp01(float value)
        {
            return Clamp(value, 0, 1);
        }

        public static float Clamp(float value, float min, float max)
        {
            if (min >= max)
            {
                throw new ArgumentException($"Min and max can not be equal; Min = {min}; Max = {max}");
            }

            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }

        public static int Clamp(int value, int min, int max)
        {
            if (min >= max)
            {
                throw new ArgumentException($"Min and max can not be equal; Min = {min}; Max = {max}");
            }

            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }

        public static bool IsNormal(this float f)
        {
            //TODO: replace to float.IsNormal() doesnt work

            return !float.IsNaN(f) && !float.IsNegativeInfinity(f) && !float.IsPositiveInfinity(f);
        }

        public static float Step(float edge, float x)
        {
            if (x > edge)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
