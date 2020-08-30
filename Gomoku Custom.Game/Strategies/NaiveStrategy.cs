using Gomoku_Custom.Game.Checkers;
using Gomoku_Custom.Game.FieldControllers;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game.Strategies
{
    public class NaiveStrategy : IStrategy
    {
        private Team[,] _internalField;
        private readonly Team _team;
        private readonly Team _enemyTeam;
        private IFieldController _controller;
        private IChecker _checker;
        public NaiveStrategy(GameData gd, Team holderTeam)
        {
            int size = gd.Field.GetLength(0);
            _internalField = new Team[size, size];
            _controller = new FieldController(_internalField);

            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j)
                    _internalField[i, j] = gd.Field[i, j];

            _team = holderTeam;
            _enemyTeam = _team == Team.Red ? Team.Blue : Team.Red;
        }
        public Point Predict(GameData gd)
        {
            return new Point();
        }

        private double Negamax(/*ref Team[,] field, */int depthLevel = 0, int multiplier = 1)
        {
            Team win = IsFinal();
            if (win != Team.None)
                return win == _team ? double.PositiveInfinity : double.NegativeInfinity;

            if (depthLevel == 0)
                return multiplier * EvalField(/*ref field*/);
            double maxScore = double.NegativeInfinity;

            foreach (Point predicts in AvailableMoves())
            {
                _controller.SetPos(predicts, multiplier == 1 ? _team : _enemyTeam);
                maxScore = Math.Max(maxScore, -Negamax(depthLevel - 1, -multiplier));
                _controller.SetPos(predicts, Team.None);
            }
            return maxScore;
        }
        private Team IsFinal()
        {
            for (int i = 0; i < _controller.FieldSize; ++i)
                for (int j = 0; j < _controller.FieldSize; ++j)
                {
                    if (_controller.GetPos(j, i) == Team.None)
                        continue;
                    Point current = new Point(i, j);
                    if (_checker.IsWinCondition(current))
                        return _controller.GetPos(current);
                }
            return Team.None;
        }
        private bool IsCandidateForMove(int x, int y)
        {
            for (int dx = -1; dx <= 1; ++dx)
                for (int dy = -1; dy <= 1; ++dy)
                {
                    if (dx == dy && dy == 0)
                        continue;
                    int px = x + dx;
                    int py = y + dy;
                    if (!_controller.IsOutOfRange(px, py) && _controller.GetPos(px, py) != Team.None)
                        return true;
                }
            return false;
        }
        private IEnumerable<Point> AvailableMoves()
        {
            List<Point> list = new List<Point>();
            for (int x = 0; x < _controller.FieldSize; ++x)
                for (int y = 0; y < _controller.FieldSize; ++y)
                    if (IsCandidateForMove(x, y))
                        list.Add(new Point(x, y));
            return list;
        }
        private double EvalField()
        {
            throw new NotImplementedException();
        }
    }
}
