using Gomoku_Custom.Game.Rules;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game.FieldControllers
{
    public class FieldController : IFieldController
    {
        private Team[][] _field;
        public int FieldSize { get; }
        public FieldController(Team[][] field)
        {
            _field = field;
            FieldSize = field.GetLength(0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Team GetPos(Point p) => GetPos(p.X, p.Y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Team GetPos(int x, int y) => _field[y][x];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsOutOfRange(Point p) =>
    IsOutOfRange(p.X, p.Y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsOutOfRange(int x, int y) =>
            y < 0 || y >= FieldSize || x < 0 || x >= FieldSize;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPos(Point p, Team value) => SetPos(p.X, p.Y, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPos(int x, int y, Team value) => _field[y][x] = value;
        public void ClearField()
        {
            for (int i = 0; i < FieldSize; ++i)
                for (int j = 0; j < FieldSize; ++j)
                    _field[i][j] = Team.None;
            //SetPos(i, j, Team.None);
        }
        public bool IsEmpty()
        {
            for (int i = 0; i < FieldSize; ++i)
                for (int j = 0; j < FieldSize; ++j)
                    if (_field[i][j] != Team.None)
                        return false;
            return true;
        }
        public bool IsFull()
        {
            for (int i = 0; i < FieldSize; ++i)
                for (int j = 0; j < FieldSize; ++j)
                    if (_field[i][j] == Team.None)
                        return false;
            return true;
        }

    }
}
