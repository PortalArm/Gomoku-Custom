using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game.Checkers
{
    public interface IChecker
    {
        int WinLength { get; }
        bool IsWinCondition(Point lastMove);
        bool LineCheck(Point basePos, Point dp);
        int LineCount(Point basePos, Point dp);
    }
}
