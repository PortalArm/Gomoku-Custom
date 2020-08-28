using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Gomoku_Custom.Game
{

    public interface IRule
    {
        public string Description { get; }
        public bool CheckRule(int turn, Point pos);
    }
    public class BasicRule : IRule
    {
        public string Description => @"На первом ходу игрок обязан поставить камень в центр поля.
Следующий игрок обязан поставить камень вокруг первого камня (8 возможных позиций).
Следующий игрок обязан поставить камень на расстоянии минимум 3 позиции.";
        private int _fieldSize;
        private byte _center;

        public BasicRule(int fieldSize)
        {
            if (fieldSize < 7 || fieldSize % 2 == 0)
                throw new Exception($"Rule is incompatible with field size of {fieldSize}");
            _fieldSize = fieldSize;
            _center = (byte)(_fieldSize / 2);
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
    public class GomokuGame
    {
        public const int FieldSize = 19;
        public Team[,] Field { get; set; }
        public Team PlayerTurn { get; set; }
        public int TurnNumber { get; set; }
        private IRule _rule { get; }
        public GomokuGame(IRule rule, Team firstTurn = Team.Blue)//, int fieldSize = 19)
        {
            Field = new Team[FieldSize, FieldSize];
            PlayerTurn = firstTurn;
            TurnNumber = 0;
            _rule = rule;
        }

        private void NextTurn()
        {
            PlayerTurn = PlayerTurn == Team.Red ? Team.Blue : Team.Red;
            ++TurnNumber;
        }
        private Team GetPos(Point pos) => Field[pos.Y, pos.X];
        private void SetPos(Point pos, Team value) => Field[pos.X, pos.Y] = value;
        private ResponseCode Validate(Team team, Point pos)
        {
            if (PlayerTurn != team)
                return ResponseCode.WrongTurn;
            if (IsOutOfRange(pos))
                return ResponseCode.PointOutOfRange;
            if (GetPos(pos) != Team.Unknown)
                return ResponseCode.PointOccupied;
            if (!_rule.CheckRule(TurnNumber, pos))
                return ResponseCode.RuleViolation;
            return ResponseCode.OK;
        }

        private static bool IsOutOfRange(Point pos) =>
            IsOutOfRange(pos.X, pos.Y);
        private static bool IsOutOfRange(int x, int y) =>
            y is < 0 or >= FieldSize || x is < 0 or >= FieldSize;
        public GameData TryMakeMove(Team team, Point pos, out ResponseCode code)
        {
            code = Validate(team, pos);
            if (code != ResponseCode.OK)
                return new GameData { Code = code, Field = Field, NextPlayer = team };

            MakeMove(team, pos);
            if (IsWinCondition(pos))
            {
                code = ResponseCode.Win;
                // TODO: Stop the game.
            }
            return new GameData {Code = code, Field = Field, NextPlayer = PlayerTurn, Updated = pos };
        }

        private bool IsWinCondition(Point lastMove)
        {
            Team team = GetPos(lastMove);
            List<Point> visited = new List<Point>();
            return false;
        }
        private void MakeMove(Team team, Point pos)
        {
            SetPos(pos, team);
            NextTurn();
        }
    }

    public class ConsoleGomokuFormatter
    {
        public static readonly Dictionary<Team, char> DefaultChars = new Dictionary<Team, char>()
            { {Team.Red, 'o' }, { Team.Blue, 'x' } };
        //private readonly GomokuGame _game;
        private StringBuilder _sb;
        private readonly Dictionary<Team, char> _teamChars;
        public ConsoleGomokuFormatter(Dictionary<Team, char> correspondances)
        {
            _teamChars = correspondances;
        }
        private void Init(int fieldSize)
        {
            int side = fieldSize + 2;
            _sb?.Clear();
            _sb ??= new StringBuilder(side * (side + 1));
            for (int i = 0; i < side; ++i)
            {
                _sb.Append(' ', side);
                _sb.Append('\n');
            }
        }
        public string RenderAsString(GameData gd)
        {
            int side = gd.Field.GetLength(0) + 2;
            if (_sb.Length < side * (side + 1))
                Init(gd.Field.GetLength(0));

            for (int i = 0; i < side - 2; ++i)
                for (int j = 0; j < side - 2; ++j)
                    _sb[(i + 1) * (side + 1) + j + 1] = _teamChars[gd.Field[i, j]];
            return _sb.ToString();
        }
        //public void Print()
        //{
        //    int side = _game.Field.GetLength(0) + 2;

        //    for (int i = 0; i < side - 2; ++i)
        //        for (int j = 0; j < side - 2; ++j)
        //            _sb[(i + 1) * (side + 1) + j + 1] = _teamChars(_game.Field[i, j]);
        //    Console.WriteLine(_sb.ToString());
        //}
    }
    class Program
    {
        static void Main(string[] args)
        {
            GomokuGame gg = new GomokuGame(new BasicRule(GomokuGame.FieldSize));
            ConsoleGomokuFormatter cgf = new ConsoleGomokuFormatter(ConsoleGomokuFormatter.DefaultChars);
            gg.TryMakeMove(Team.Blue, new Point(9, 9), out var code); Console.WriteLine(code); 
            gg.TryMakeMove(Team.Red, new Point(8, 9), out code); Console.WriteLine(code); 
            gg.TryMakeMove(Team.Blue, new Point(3, 9), out code); Console.WriteLine(code);
            gg.TryMakeMove(Team.Red, new Point(1, 1), out code); Console.WriteLine(code); 
            gg.TryMakeMove(Team.Blue, new Point(9, 9), out code); Console.WriteLine(code);
            gg.TryMakeMove(Team.Red, new Point(9, 9), out code); Console.WriteLine(code); 
            gg.TryMakeMove(Team.Blue, new Point(9, 9), out code); Console.WriteLine(code);
            gg.TryMakeMove(Team.Red, new Point(9, 9), out code); Console.WriteLine(code); 
            gg.TryMakeMove(Team.Blue, new Point(9, 9), out code); Console.WriteLine(code);
            gg.TryMakeMove(Team.Red, new Point(9, 9), out code); Console.WriteLine(code);
            //BasicRule br = new BasicRule(19);
            //Console.WriteLine(br.CheckRule(1, new Point(9,9)));
            //Console.WriteLine(br.CheckRule(1, new Point(231,8)));
            //Console.WriteLine(br.CheckRule(1, new Point(8,8)));
            //Console.WriteLine(br.CheckRule(2, new Point(8,8)));
            //Console.WriteLine(br.CheckRule(2, new Point(9,9)));
            //Console.WriteLine(br.CheckRule(2, new Point(10,7)));
            //Console.WriteLine(br.CheckRule(2, new Point(10,8)));
            //Console.WriteLine(br.CheckRule(3, new Point(9,9)));
            //Console.WriteLine(br.CheckRule(3, new Point(10,10)));
            //Console.WriteLine(br.CheckRule(3, new Point(11,11)));
            //Console.WriteLine(br.CheckRule(3, new Point(12,12)));

        }
    }
}
