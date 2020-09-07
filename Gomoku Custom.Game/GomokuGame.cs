using Gomoku_Custom.Game.Checkers;
using Gomoku_Custom.Game.FieldControllers;
using Gomoku_Custom.Game.Rules;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Gomoku_Custom.Game
{

    public class GomokuGameBuilder
    {
        private GomokuGame _game;
        private GomokuPlayer _playerBlue, _playerRed;
        private GomokuGameBuilder() { }
        public static GomokuGameBuilder Create(int fieldSize, int winLength, IRule rule)
        {
            return new GomokuGameBuilder {
                _game = new GomokuGame(rule, Team.Blue, fieldSize, winLength)
            };
        }

        //public GomokuGameBuilder SetupBluePlayer() {
        //    new GomokuPlayer();
        //}

        public class GomokuGamePlayerChoose
        {
            public GomokuGamePlayerChoose() { }
        }
    }
    public class GomokuGameRunner
    {
        public GomokuPlayer PlayerBlue { get; }
        public GomokuPlayer PlayerRed { get; }
        public GomokuGame Game { get; }
        public GomokuGameRunner()
        {
            Console.WriteLine("Runner created!");
        }

        public void Start() {
            Console.WriteLine("Runner started the game!");
        }
    }
    public class GomokuGame
    {
        public int FieldSize { get; }
        public int WinLength { get; }
        private IFieldController _controller;
        private IChecker _checker;
        private IRule _rule;
        public Team[][] Field { get; private set; }
        public Team PlayerTurn { get; private set; }
        public int TurnNumber { get; private set; }

        public GameState State { get; private set; }
        public GomokuGame(IRule rule, Team firstTurn = Team.Blue, int fieldSize = 19, int winLength = 5)
        {
            FieldSize = fieldSize;
            WinLength = winLength;
            Field = new Team[FieldSize][];
            for (int i = 0; i < FieldSize; ++i)
                Field[i] = new Team[FieldSize];
            _controller = new FieldController(Field);
            _checker = new Checker(_controller, WinLength);
            PlayerTurn = firstTurn;
            TurnNumber = 0;
            _rule = rule;

            var d = new GomokuGameBuilder.GomokuGamePlayerChoose();
        }

        public GameData StartGame(Team first)
        {
            _controller.ClearField();
            PlayerTurn = first;
            TurnNumber = 0;
            State = GameState.GameStarted;
            return new GameData { Code = ResponseCode.OK, Field = Field, NextPlayer = PlayerTurn, Updated = Point.Empty };
        }
        //public GameData StartRealGame(Team first) {

        //}
        private void NextTurn()
        {
            PlayerTurn = PlayerTurn == Team.Red ? Team.Blue : Team.Red;
            ++TurnNumber;
        }
        private ResponseCode Validate(Team team, Point pos)
        {
            if (PlayerTurn != team)
                return ResponseCode.WrongTurn;
            if (_controller.IsOutOfRange(pos))
                return ResponseCode.PointOutOfRange;
            if (!_rule.CheckRule(TurnNumber, pos))
                return ResponseCode.RuleViolation;
            if (_controller.GetPos(pos) != Team.None)
                return ResponseCode.PointOccupied;
            return ResponseCode.OK;
        }
        public GameData TryMakeMove(Team team, Point pos)//, out ResponseCode code)
        {
            ResponseCode code = Validate(team, pos);
            if (code != ResponseCode.OK)
                return new GameData { Code = code, Field = Field, NextPlayer = team, Updated = Point.Empty };

            MakeMove(team, pos);
            bool isWin = _checker.IsWinCondition(pos);
            if (isWin || _controller.IsFull())
            {
                code = isWin ? ResponseCode.Win : ResponseCode.Draw;
                State = GameState.GameEnded;
                // TODO: Stop the game.
            }
            return new GameData { Code = code, Field = Field, NextPlayer = PlayerTurn, Updated = pos };
        }
        private void MakeMove(Team team, Point pos)
        {
            _controller.SetPos(pos, team);
            NextTurn();
        }
    }
}
