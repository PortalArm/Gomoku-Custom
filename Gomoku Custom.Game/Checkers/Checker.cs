using Gomoku_Custom.Game.FieldControllers;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game.Checkers
{
    public class Checker : IChecker
    {
        private IFieldController _controller;
        private int _winLength;
        public Checker(IFieldController controller, int winLength)
        {
            _controller = controller;
            _winLength = winLength;
        }

        private bool LineCheck(Point basePos, Point dp)
        {
            if (_controller.IsOutOfRange(basePos) || _controller.GetPos(basePos) == Team.None)
                throw new ArgumentOutOfRangeException();
            Team team = _controller.GetPos(basePos);
            int count = 1;
            // Считаем в обе стороны, пока не выйдем за границы / не найдём не нашу клетку 
            // TODO: Refactor
            for (int i = 1; i <= _winLength; ++i)
            {
                Point checkedPos = basePos + dp * i;
                if (_controller.IsOutOfRange(checkedPos) || _controller.GetPos(checkedPos) != team)
                    break;
                count += 1;
            }
            for (int i = 1; i <= _winLength/*+1-count*/; ++i)
            {
                Point checkedPos = basePos - dp * i;
                if (_controller.IsOutOfRange(checkedPos) || _controller.GetPos(checkedPos) != team)
                    break;
                count += 1;
            }
            return count == _winLength;
        }
        private static List<Point> _directions = new List<Point>{
                new Point(1, 1),
                new Point(-1, 1),
                new Point(1, 0),
                new Point(0, 1)};

        public bool IsWinCondition(Point lastMove)
        {
            foreach (Point dir in _directions)
                if (LineCheck(lastMove, dir))
                    return true;
            return false;
        }

    }
}
