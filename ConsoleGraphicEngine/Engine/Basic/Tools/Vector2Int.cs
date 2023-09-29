using System;
using System.Numerics;

namespace ConsoleGraphicEngine3D.Engine.Basic.Tools
{
    /// <summary>
    /// Struct stores x and y position
    /// </summary>
    public struct Vector2Int
    {
        public int X;
        public int Y;

        public Vector2Int(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Vector2Int Zero => new Vector2Int(0, 0);
        public static Vector2Int One => new Vector2Int(0, 0);
        public static Vector2Int axisX => new Vector2Int(1, 0);
        public static Vector2Int axisY => new Vector2Int(0, 1);


        public static Vector2Int operator -(Vector2Int a)
        {
            return new Vector2Int(-a.X, -a.Y);
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2Int operator *(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X * b.X, a.Y * b.Y);
        }

        public static Vector2Int operator *(Vector2Int a, int multiplier)
        {
            return new Vector2Int(a.X * multiplier, a.Y * multiplier);
        }

        public static Vector2 operator *(Vector2Int a, float multiplier)
        {
            return new Vector2(a.X * multiplier, a.Y * multiplier);
        }

        public static Vector2Int operator /(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X / b.X, a.Y / b.Y);
        }

        public static Vector2Int operator /(Vector2Int a, int dividor)
        {
            return new Vector2Int(a.X / dividor, a.Y / dividor);
        }

        public static Vector2 operator /(Vector2Int a, float dividor)
        {
            return new Vector2(a.X / dividor, a.Y / dividor);
        }

        public static bool operator >(Vector2Int a, Vector2Int b)
        {
            return a.X > b.X && a.Y > b.Y;
        }

        public static bool operator <(Vector2Int a, Vector2Int b)
        {
            return a.X < b.X && a.Y < b.Y;
        }

        public static bool operator >=(Vector2Int a, Vector2Int b)
        {
            return a.X >= b.X && a.Y >= b.Y;
        }

        public static bool operator <=(Vector2Int a, Vector2Int b)
        {
            return a.X <= b.X && a.Y <= b.Y;
        }

        public static bool operator ==(Vector2Int a, Vector2Int b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2Int a, Vector2Int b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"({X}; {Y})";
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2Int otherVector &&
                   X == otherVector.X &&
                   Y == otherVector.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
