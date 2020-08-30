using Gomoku_Custom.Shared;
using System;

namespace Gomoku_Custom.Game.Rules
{
    public class BasicRule : IRule
    {
        public string Description => @"На первом ходу игрок обязан поставить камень в центр поля.
Следующий игрок обязан поставить камень вокруг первого камня (8 возможных позиций).
Следующий игрок обязан поставить камень на расстоянии минимум 3 позиции.";
        private int _fieldSize;
        private int _center;

        public BasicRule(int fieldSize)
        {
            if (fieldSize < 7 || fieldSize % 2 == 0)
                throw new Exception($"Rule is incompatible with field size of {fieldSize}");
            _fieldSize = fieldSize;
            _center = _fieldSize / 2;
        }
        public bool CheckRule(int turn, Point pos)
        {
            int dx = Math.Abs(pos.X - _center);
            int dy = Math.Abs(pos.Y - _center);
            return (turn, pos) switch
            {
                (0, _) when pos.X == pos.Y && pos.X == _center => true,
                (1, _) when dx == 1 && dy <= 1 || dx <= 1 && dy == 1 => true,
                (2, _) when dx >= 3 || dy >= 3 => true,
                _ => false
            };
        }
    }
}
