namespace ConsoleGraphicEngine.Engine.Tools
{
    /// <summary>
    /// Struct stores x and y position
    /// </summary>
    internal struct Vector2Int
    {
        public Vector2Int(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X;
        public int Y;

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

        public static Vector2Int operator /(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X / b.X, a.Y / b.Y);
        }

        public static Vector2Int operator /(Vector2Int a, int dividor)
        {
            return new Vector2Int(a.X / dividor, a.Y / dividor);
        }

        public static bool operator ==(Vector2Int a, Vector2Int b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2Int a, Vector2Int b)
        {
            return !(a == b);
        }
    }
}
