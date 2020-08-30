using Gomoku_Custom.Game.Rules;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game.FieldControllers
{
    public class FieldController : IFieldController
    {
        private Team[,] _field;
        public int FieldSize { get; }
        private IRule _rule;
        public FieldController(Team[,] field)
        {
            _field = field;
            FieldSize = field.GetLength(0);
        }

        public Team GetPos(Point p) => GetPos(p.X, p.Y);
        public Team GetPos(int x, int y) => _field[y, x];
        public bool IsOutOfRange(Point p) =>
    IsOutOfRange(p.X, p.Y);
        public bool IsOutOfRange(int x, int y) =>
            y < 0 || y >= FieldSize || x < 0 || x >= FieldSize;
        public void SetPos(Point p, Team value) => _field[p.Y, p.X] = value;
        public void ClearField()
        {
            for (int i = 0; i < FieldSize; ++i)
                for (int j = 0; j < FieldSize; ++j)
                    _field[i, j] = Team.None;
        }
    }
}
