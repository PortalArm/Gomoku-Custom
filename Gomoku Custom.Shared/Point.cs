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

    }
}
