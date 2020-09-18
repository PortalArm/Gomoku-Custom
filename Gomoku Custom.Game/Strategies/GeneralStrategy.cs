using Gomoku_Custom.Game.Checkers;
using Gomoku_Custom.Game.FieldControllers;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game.Strategies
{
    [Obsolete]
    public class GeneralStrategy : IStrategy
    {
        public bool IsHuman => false;
        private Team[][] _field;
        private int _fieldSize;
        private readonly Team _team;
        private readonly Team _enemyTeam;
        private int _winLength;
        //private IFieldController _controller;
        //private IChecker _checker;
        private int _depth;
        private static readonly Random _rand = new Random();
        public GeneralStrategy(GameData gd, Team holderTeam, int winLength, int maxDepth = 6)
        {
            int size = gd.Field.Length;
            _field = new Team[size][];
            for (int i = 0; i < size; ++i)
                _field[i] = new Team[size];
            _fieldSize = size;
            _winLength = winLength;
            //_controller = new FieldController(_field);
            //_checker = new Checker(_controller, winLength);
            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j)
                    _field[i][j] = gd.Field[i][j];

            _team = holderTeam;
            _enemyTeam = OpponentOf(_team);//_team == Team.Red ? Team.Blue : Team.Red;
            _depth = maxDepth;
        }
        public void UpdateState(GameData gd)
        {
            if (gd.Updated != Point.Empty)
                _field[gd.Updated.Y][gd.Updated.X] = gd.Field[gd.Updated.Y][gd.Updated.X];
        }
        public bool IsEmpty()
        {
            for (int i = 0; i < _fieldSize; ++i)
                for (int j = 0; j < _fieldSize; ++j)
                    if (_field[i][j] != Team.None)
                        return false;
            return true;
        }
        public bool IsFull()
        {
            for (int i = 0; i < _fieldSize; ++i)
                for (int j = 0; j < _fieldSize; ++j)
                    if (_field[i][j] == Team.None)
                        return false;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsOutOfRange(Point p) => IsOutOfRange(p.X, p.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsOutOfRange(int x, int y) =>
    y < 0 || y >= _fieldSize || x < 0 || x >= _fieldSize;
        public Point UpdateAndPredict(GameData gd)
        {
            UpdateState(gd);
            if (IsEmpty())
                return new Point(_rand.Next(0, _fieldSize), _rand.Next(0, _fieldSize));
            Point predicted = Predict();
            if (predicted == Point.Empty)
            {
                var moves = AvailableMoves();
                return moves[_rand.Next(moves.Count)];
            }

            return predicted;

        }

        private Point Predict()
        {
            Point move = Point.Empty;
            var moves = AvailableMoves();
            double score = -maxValue;
            foreach (Point next in moves)
            {
                SetPos(next, _team);
                double newScore = Minimax(next, false, 0);
                File.AppendAllText("output.txt", $"For move {next} best score is {newScore}\n");
                SetPos(next, Team.None);
                if (newScore > score)
                {
                    score = newScore;
                    move = next;
                }
            }
            return move;
        }
        private static readonly double maxValue = 32000;//double.PositiveInfinity;
        private static Team OpponentOf(Team team) => team == Team.Red ? Team.Blue : Team.Red;
        private Team GetWinner()
        {
            for (int y = 0; y < _fieldSize; ++y)
                for (int x = 0; x < _fieldSize; ++x)
                {
                    if (GetPos(x, y) == Team.None)
                        continue;
                    Point current = new Point(x, y);
                    if (IsWinCondition(current))
                        return GetPos(current);
                }
            return Team.None;
        }
        private Team GetPos(Point point) => GetPos(point.X, point.Y);
        private Team GetPos(int x, int y) => _field[y][x];
        public bool LineCheck(Point basePos, Point dp) =>
    LineCount(basePos, dp) == _winLength;
        public int LineCount(Point basePos, Point dp)
        {
            if (IsOutOfRange(basePos) || GetPos(basePos) == Team.None)
                throw new ArgumentOutOfRangeException();
            Team team = GetPos(basePos);
            int count = 1;
            // Считаем в обе стороны, пока не выйдем за границы / не найдём не нашу клетку 
            // TODO: Refactor
            for (int i = 1; i <= _winLength; ++i)
            {
                Point checkedPos = basePos + dp * i;
                if (IsOutOfRange(checkedPos) || GetPos(checkedPos) != team)
                    break;
                count += 1;
            }
            for (int i = 1; i <= _winLength/*+1-count*/; ++i)
            {
                Point checkedPos = basePos - dp * i;
                if (IsOutOfRange(checkedPos) || GetPos(checkedPos) != team)
                    break;
                count += 1;
            }
            return count;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetPos(Point p, Team val) => SetPos(p.X, p.Y, val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetPos(int x, int y, Team val) => _field[y][x] = val;
        private double Minimax(Point added, bool isMyTeam, int depth)
        {
            Team team = GetWinner();
            if (team != Team.None)
                return team == _team ? maxValue - depth : -maxValue + depth;
            else
                if (IsFull())
                return 0;

            List<Point> moves = AvailableMoves();
            if (isMyTeam)
            {
                double maxScore = -maxValue;
                foreach (Point next in moves)
                {
                    SetPos(next, _team);
                    maxScore = Math.Max(maxScore, Minimax(next, false, depth + 1));
                    SetPos(next, Team.None);

                }
                return maxScore;
            } else
            {
                double minScore = maxValue;
                foreach (Point next in moves)
                {
                    SetPos(next, _enemyTeam);
                    minScore = Math.Min(minScore, Minimax(next, true, depth + 1));
                    SetPos(next, Team.None);
                }
                return minScore;
            }
        }
        private List<Point> AvailableMoves()
        {
            List<Point> list = new List<Point>();
            for (int y = 0; y < _fieldSize; ++y)
                for (int x = 0; x < _fieldSize; ++x)
                    if (IsCandidateForMove(x, y))
                        //yield return new Point(x, y);
                        list.Add(new Point(x, y));

            return list;
        }
        private bool IsCandidateForMove(int x, int y)
        {
            if (IsOutOfRange(x, y) || GetPos(x, y) != Team.None)
                return false;

            for (int dx = -1; dx <= 1; ++dx)
                for (int dy = -1; dy <= 1; ++dy)
                {
                    if (dx == 0 && dy == 0)
                        continue;
                    int px = x + dx;
                    int py = y + dy;
                    if (!IsOutOfRange(px, py) && GetPos(px, py) != Team.None)
                        return true;
                }
            return false;
        }
    }
}
