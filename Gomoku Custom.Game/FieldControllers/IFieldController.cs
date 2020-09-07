using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game.FieldControllers
{
    public interface IFieldController
    {
        int FieldSize { get; }
        void SetPos(Point p, Team value);
        void SetPos(int x, int y, Team value);
        Team GetPos(Point p);
        Team GetPos(int x, int y);
        bool IsOutOfRange(Point p);
        bool IsOutOfRange(int x, int y);
        void ClearField();
        bool IsEmpty();
        bool IsFull();
        //    private static bool IsOutOfRange(Point pos) =>
        //IsOutOfRange(pos.X, pos.Y);
        //    private static bool IsOutOfRange(int x, int y) =>
        //        y is < 0 or >= FieldSize || x is < 0 or >= FieldSize;
    }
}
