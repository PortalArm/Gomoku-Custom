using System;
using System.Runtime.CompilerServices;

namespace Gomoku_Custom.Shared
{
    [Serializable]
    public readonly struct Point
    {
        public int X { get; }
        public int Y { get; }
        public static readonly Point Empty = new Point(-1,-1);
        public Point(int x, int y) =>
            (X, Y) = (x, y);

        public static bool operator ==(Point a, Point b) =>
            (a.X, a.Y) == (b.X, b.Y);
        public static bool operator !=(Point a, Point b) =>
            !(a == b);
        public static Point operator *(Point a, int k) =>
            new Point(a.X * k, a.Y * k);

        public static Point operator +(Point a, Point b) =>
            new Point(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) =>
            new Point(a.X - b.X, a.Y - b.Y);
        public static Point operator *(int k, Point a) => a * k;

        public override string ToString() => $"({X}, {Y})";
        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
