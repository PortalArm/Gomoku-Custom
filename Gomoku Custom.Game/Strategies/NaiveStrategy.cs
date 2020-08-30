using Gomoku_Custom.Game.Checkers;
using Gomoku_Custom.Game.FieldControllers;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            _checker = new Checker(_controller, GomokuGame.WinLength);
            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j)
                    _internalField[i, j] = gd.Field[i, j];

            _team = holderTeam;
            _enemyTeam = _team == Team.Red ? Team.Blue : Team.Red;
        }
        public Point Predict(GameData gd)
        {
            _controller.SetPos(gd.Updated, gd.Field[gd.Updated.Y, gd.Updated.X]);
            if (_controller.IsEmpty())
                return new Point(_rand.Next(0, _controller.FieldSize), _rand.Next(0, _controller.FieldSize));
            Negamax(1,1, double.NegativeInfinity, double.PositiveInfinity);
            return _predicted;
        }
        private Point _predicted;
        private double Negamax(/*ref Team[,] field, */int depthLevel, int multiplier, double alpha, double beta)
        {
            Team win = IsFinal();
            if (win != Team.None)
                return win == _team ? double.PositiveInfinity : double.NegativeInfinity;

            if (depthLevel == 0)
                return multiplier * EvalField(/*ref field*/);
            double maxScore = double.NegativeInfinity;

            foreach (Point predict in AvailableMoves())
            {
                _controller.SetPos(predict, multiplier == 1 ? _team : _enemyTeam);
                double newScore = -Negamax(depthLevel - 1, -multiplier,-beta, -alpha);
                _controller.SetPos(predict, Team.None);

                if (newScore > maxScore)
                {
                    maxScore = newScore;
                    _predicted = predict;
                }
                alpha = Math.Max(alpha, maxScore);
                if (alpha >= beta)
                    break;
                //maxScore = Math.Max(maxScore, -Negamax(depthLevel - 1, -multiplier));
            }
            return maxScore;
        }
        private Team IsFinal()
        {
            for (int i = 0; i < _controller.FieldSize; ++i)
                for (int j = 0; j < _controller.FieldSize; ++j)
                {
                    if (_controller.GetPos(i, j) == Team.None)
                        continue;
                    Point current = new Point(i, j);
                    if (_checker.IsWinCondition(current))
                        return _controller.GetPos(current);
                }
            return Team.None;
        }
        private bool IsCandidateForMove(int x, int y)
        {
            if (!_controller.IsOutOfRange(x, y) && _controller.GetPos(x, y) != Team.None)
                return false;

            for (int dx = -1; dx <= 1; ++dx)
                for (int dy = -1; dy <= 1; ++dy)
                {
                    if (dx == 0 && dy == 0)
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
        private static readonly Random _rand = new Random();
        private double EvalField()
        {
            //TODO
            return _rand.NextDouble();
        }
    }

}
