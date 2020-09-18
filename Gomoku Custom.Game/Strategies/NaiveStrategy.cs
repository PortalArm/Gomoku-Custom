using Gomoku_Custom.Game.Checkers;
using Gomoku_Custom.Game.FieldControllers;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using Gomoku_Custom.Game.Formatters;
namespace Gomoku_Custom.Game.Strategies
{
    public class NaiveStrategy : IStrategy
    {
        public bool IsHuman => false;
        private Team[][] _internalField;
        private readonly Team _team;
        private IFieldController _controller;
        private IChecker _checker;
        private int _depth;
        //public NaiveStrategy(GameData gd, Team team, int winLength, int maxDepth = 6)
        public NaiveStrategy(int fieldSize, Team team, int winLength, int maxDepth = 6)
        {
            int size = fieldSize;
            _internalField = new Team[size][];
            for (int i = 0; i < size; ++i)
                _internalField[i] = new Team[size];

            _controller = new FieldController(_internalField);
            _checker = new Checker(_controller, winLength);

            _team = team;
            _depth = maxDepth;
        }
        public void UpdateState(GameData gd)
        {
            if (gd.Updated != Point.Empty)
                _controller.SetPos(gd.Updated, gd.Field[gd.Updated.Y][gd.Updated.X]);
        }

        public Point UpdateAndPredict(GameData gd)
        {
            UpdateState(gd);

            //File.AppendAllText("output.txt", $"---------------------------------NEW MOVE-{gd.Updated}-------------------------------------------\n");
            //File.AppendAllText("output.txt", cgf.RenderAsString(_controller));
            //File.AppendAllText("output.txt", $"Our team: {_team}, symbol: {cgf.GetSymbolOf(_team)}\n");
            if (_controller.IsEmpty())
                return new Point(_rand.Next(0, _controller.FieldSize), _rand.Next(0, _controller.FieldSize));
            Point predicted = Predict();//Minimax(gd.Updated, _team, 0, -maxValue, maxValue);
            if (predicted != Point.Empty)
                return predicted;
            
            var moves = AvailableMoves();
            return moves[_rand.Next(moves.Count)];
        }

        private Point Predict()
        {
            Point move = Point.Empty;
            var moves = AvailableMoves();
            double score = -maxValue;
            Team opponent = OpponentOf(_team);
            foreach (Point next in moves)
            {
                _controller.SetPos(next, _team);
                double newScore = -Minimax(next, CreateMoves(moves, next), opponent, 0, -maxValue, maxValue);
                _controller.SetPos(next, Team.None);
                if (newScore > score)
                {
                    score = newScore;
                    move = next;
                }
            }
            return move;
        }
        // xoooox = ok
        // _oooo_ >= 0 = auto loss
        // xoooo_ == 1 = auto place x on _
        // xoooo_ >= 1 = auto loss
        // _ooo_ >= 2 = auto loss
        // _ooo_ == 1 = auto place x on _
        // xooo_ ...
        private static readonly double maxValue = 32000;//double.PositiveInfinity;
        private static Team OpponentOf(Team team) => team == Team.Red ? Team.Blue : Team.Red;

        private double Minimax(Point added, List<Point> moves, Team player, int depth, double a, double b)
        {
            Team team = GetWinner(added);
            if (team != Team.None)
                return team == player ? maxValue - depth : -maxValue + depth;
            else
                if (_controller.IsFull())
                return 0;

            if (depth == _depth)
                return EvalField(added);

            //List<Point> moves = AvailableMoves();
            double maxScore = -maxValue;
            foreach (Point next in moves)
            {
                _controller.SetPos(next, player);
                maxScore = Math.Max(maxScore, -Minimax(next, CreateMoves(moves, next), OpponentOf(player), depth + 1, -b, -a));
                _controller.SetPos(next, Team.None);

                a = Math.Max(a, maxScore);
                if (a >= b)
                    break;
            }
            return maxScore;
        }


        private double EvalField(Point p)
        {
            return 0;
        }
        //private PointScore EvalField(Point p, Team player)
        //{
        //    //return _rand.NextDouble();
        //    Team winner = GetWinner(p);
        //    //File.AppendAllText("output.txt", cgf.RenderAsString(_controller));
        //    //File.AppendAllText("output.txt", $"Evaluating from point {p} with mult == {mult}\n");
        //    //File.AppendAllText("output.txt", $"Our team: {_team}, symbol: {cgf.GetSymbolOf(_team)}\n");
        //    //File.AppendAllText("output.txt", $"Final score: {(winner != Team.None ? (winner == _team ? maxValue : -maxValue) : 0)}\n");
        //    if (winner != Team.None)
        //        return new PointScore(winner == player ? maxValue : -maxValue, p);
        //    //_controller.SetPos(p, OpponentOf(player));
        //    //winner = GetWinner(p);
        //    //_controller.SetPos(p, Team.None);
        //    //if(winner == OpponentOf(player))
        //    //    return new PointScore(maxValue, p);
        //    return new PointScore(0, p);

        //    //Dictionary<Point, Direction> processed = new Dictionary<Point, Direction>();
        //    //int[] counts = new int[_checker.WinLength];
        //    //for (int y = 0; y < _controller.FieldSize; ++y)
        //    //    for (int x = 0; x < _controller.FieldSize; ++x)
        //    //    {
        //    //        Point current = new Point(x, y);
        //    //        Team team = _controller.GetPos(current);
        //    //        int offset = team == _team ? 1 : -1;
        //    //        //if(_internalField[current.Y,current.X] != team)
        //    //        if (_controller.GetPos(current) == Team.None)
        //    //            continue;

        //    //        //Если текущая клетка была в буфере обработанных, извлекаем, по каким направлениям она была обработана
        //    //        Direction processedDirections = Direction.None;
        //    //        if (processed.ContainsKey(current))
        //    //            processedDirections = processed[current];

        //    //        //REFACTOR
        //    //        foreach (var vector in Vectors)
        //    //        {
        //    //            if (processedDirections.HasFlag(vector.Key))
        //    //                continue;
        //    //            int count = 1;
        //    //            Point checking = current + vector.Value;
        //    //            while (!_controller.IsOutOfRange(checking) && _controller.GetPos(checking) == team)
        //    //            {
        //    //                ++count;
        //    //                if (processed.ContainsKey(current))
        //    //                    processed[current] |= vector.Key;
        //    //                else
        //    //                    processed[current] = vector.Key;

        //    //                checking += vector.Value;
        //    //            }
        //    //            if (count < _checker.WinLength)
        //    //                counts[count - 1] += offset;
        //    //            //else
        //    //            //    if (count == 5)
        //    //            //    return double.PositiveInfinity;

        //    //        }
        //    //    }
        //    //File.AppendAllText("output.txt", cgf.RenderAsString(_controller));
        //    //File.AppendAllText("output.txt", $"Our team: {_team}, symbol: {cgf.GetSymbolOf(_team)}\n");
        //    //File.AppendAllText("output.txt", $"Array cfg: {string.Join(" ", counts)}\n");
        //    ////return _rand.NextDouble();
        //    //return counts[0] * Weights[0] + counts[1] * Weights[1] + counts[2] * Weights[2];// + counts[3] * Weights[3];
        //}
        private Team GetWinner(Point winPoint)
        {
            if (_checker.IsWinCondition(winPoint))
                return _controller.GetPos(winPoint);
            return Team.None;
        }
        private bool IsCandidateForMove(int x, int y)
        {
            if (_controller.IsOutOfRange(x, y) || _controller.GetPos(x, y) != Team.None)
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
        private List<Point> CreateMoves(List<Point> @base, Point @new) {
            List<Point> result = new List<Point>(@base);
            result.Remove(@new);
            foreach (Point p in AvailableMoves(@new))
                if (!result.Contains(p))
                    result.Add(p);
            return result;
        }
        private List<Point> AvailableMoves(Point @new)
        {
            List<Point> points = new List<Point>();
            for (int dx = -1; dx <= 1; ++dx)
                for (int dy = -1; dy <= 1; ++dy)
                {
                    if (dx == 0 && dy == 0)
                        continue;
                    Point candidate = new Point(@new.X + dx, @new.Y + dy);
                    if (!_controller.IsOutOfRange(candidate) && _controller.GetPos(candidate) == Team.None)
                        points.Add(candidate);
                }
            return points;
        }
        private List<Point> AvailableMoves()
        {
            List<Point> list = new List<Point>();
            for (int y = 0; y < _controller.FieldSize; ++y)
                for (int x = 0; x < _controller.FieldSize; ++x)
                    if (IsCandidateForMove(x, y))
                        list.Add(new Point(x, y));

            return list;
        }
        //private List<Point> GetNewPoints(int x, int y)
        //{
        //    List<Point> points = new List<Point>();
        //    for (int dx = -1; dx <= 1; ++dx)
        //        for (int dy = -1; dy <= 1; ++dy)
        //        {
        //            if (dx == 0 && dy == 0)
        //                continue;
        //            int px = x + dx;
        //            int py = y + dy;
        //            Point current = new Point(px, py);
        //            if (!_controller.IsOutOfRange(px, py) &&
        //                _controller.GetPos(px, py) == Team.None &&
        //                !_availableMoves.Contains(current))
        //                points.Add(current);
        //        }
        //    return points;
        //}

        private static readonly Random _rand = new Random();

        private static double[] Weights = { 1.0, 10, 100, 1000 };
        [Flags]
        private enum Direction { None = 1, MainDiagonal = 2, SecDiagonal = 4, Horizontal = 8, Vertical = 16 };
        private static Dictionary<Direction, Point> Vectors = new Dictionary<Direction, Point>() {
            { Direction.Horizontal, new Point(1, 0) },
            { Direction.SecDiagonal, new Point(-1, 1) },
            { Direction.Vertical, new Point(0, 1) },
            { Direction.MainDiagonal, new Point(1, 1) }
        };
        private static readonly ConsoleGomokuFormatter cgf = new ConsoleGomokuFormatter();
        //private double EvalNew(int depth)
        //{
        //    int[] counts = new int[_checker.WinLength];

        //    for (int x = 0; x < _controller.FieldSize; ++x)
        //        for (int y = 0; y < _controller.FieldSize; ++y)
        //        {
        //            if (_controller.GetPos(x, y) == Team.None)
        //                continue;
        //            Point current = new Point(x, y);
        //            int offset = _controller.GetPos(current) == _team ? 1 : -1;
        //            counts[0] += offset;
        //            foreach (var vector in Vectors)
        //            {
        //                int lineCount = _checker.LineCount(current, vector.Value);
        //                if (lineCount == 1)
        //                    continue;
        //                File.AppendAllText("output.txt", $"Got {lineCount} on {vector.Key} at {current}\n");
        //                if (lineCount < _checker.WinLength)
        //                    counts[lineCount - 1] += offset;
        //                else
        //                    if (lineCount == _checker.WinLength)
        //                {
        //                    File.AppendAllText("output.txt", cgf.RenderAsString(_controller));
        //                    File.AppendAllText("output.txt", $"Our team: {_team}, symbol: {cgf.GetSymbolOf(_team)}\n");
        //                    File.AppendAllText("output.txt", "Someone won!");
        //                    return offset == 1 ? double.PositiveInfinity : double.NegativeInfinity;
        //                }
        //            }
        //        }
        //    //File.AppendAllText("output.txt", cgf.RenderAsString(_controller));
        //    //File.AppendAllText("output.txt", $"Our team: {_team}, symbol: {cgf.GetSymbolOf(_team)}, depth: {depth}\n");
        //    //File.AppendAllText("output.txt", $"Array cfg: {string.Join(" ", counts)}\n");
        //    int weight = counts.Aggregate(0, (a, b) => a + (b < 0 ? -1 : 1) * b * b);
        //    //File.AppendAllText("output.txt", $"Weight of move: {weight}\n");
        //    return weight;
        //}

        //private double EvalFieldV2()
        //{
        //    int[] counts = new int[_checker.WinLength];
        //    //int horizontals = 0, verticals = 0, main = 0, sec = 0;
        //    for (int y = 0; y < _controller.FieldSize; ++y)
        //    {
        //        int val = 0, offset = 1;
        //        for (int x = 0; x < _controller.FieldSize; ++x)
        //        {
        //            Team current = _controller.GetPos(x, y);
        //            if (current == Team.None)
        //            {
        //                if (val > 0 && val < _checker.WinLength)
        //                    counts[val - 1] += offset;
        //                continue;
        //            }
        //            if (current == _team)

        //        }
        //    }
        //}

    }

}
