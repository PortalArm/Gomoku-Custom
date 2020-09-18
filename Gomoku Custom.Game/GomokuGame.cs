using Gomoku_Custom.Game.Checkers;
using Gomoku_Custom.Game.FieldControllers;
using Gomoku_Custom.Game.Rules;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Gomoku_Custom.Game
{
    public class GomokuGame
    {
        public int FieldSize { get; }
        public int WinLength { get; }
        private readonly IFieldController _controller;
        private readonly IChecker _checker;
        private readonly IRule _rule;
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
        }

        public GameData StartGame(Team first)
        {
            _controller.ClearField();
            PlayerTurn = first;
            TurnNumber = 0;
            State = first == Team.Blue ? GameState.WaitingForBlue : GameState.WaitingForRed;
            return new GameData { Code = ResponseCode.OK, Field = Field, NextPlayer = PlayerTurn, Updated = Point.Empty };
        }
        private void NextTurn()
        {
            PlayerTurn = PlayerTurn == Team.Red ? Team.Blue : Team.Red;
            State = PlayerTurn == Team.Blue ? GameState.WaitingForBlue : GameState.WaitingForRed;
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
        public GameData TryMakeMove(Team team, Point pos)
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
