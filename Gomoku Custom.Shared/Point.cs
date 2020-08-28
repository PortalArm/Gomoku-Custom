using System;

namespace Gomoku_Custom.Shared
{
    [Serializable]
    public struct Point
    {
        public byte X { get; set; }
        public byte Y { get; set; }

        public Point(byte x, byte y) =>
            (X, Y) = (x, y);

        public static bool operator ==(Point a, Point b) =>
            (a.X, a.Y) == (b.X, b.Y);
        public static bool operator !=(Point a, Point b) =>
            !(a == b);

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
