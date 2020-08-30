using Gomoku_Custom.Game.Checkers;
using Gomoku_Custom.Game.FieldControllers;
using Gomoku_Custom.Game.Rules;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;

namespace Gomoku_Custom.Game
{
    public class GomokuGame
    {
        public const int FieldSize = 19;
        public const int WinLength = 5;
        private IFieldController _controller;
        private IChecker _checker;
        private IRule _rule;
        public Team[,] Field { get; private set; }
        public Team PlayerTurn { get; private set; }
        public int TurnNumber { get; private set; }

        public GameState State { get; private set; }
        public GomokuGame(IRule rule, Team firstTurn = Team.Blue)//, int fieldSize = 19)
        {
            Field = new Team[FieldSize, FieldSize];
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
            State = GameState.GameStarted;
            return new GameData { Code = ResponseCode.OK, Field = Field, NextPlayer = PlayerTurn };
        }
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
            ResponseCode code =  Validate(team, pos);
            if (code != ResponseCode.OK)
                return new GameData { Code = code, Field = Field, NextPlayer = team };

            MakeMove(team, pos);
            if (_checker.IsWinCondition(pos))
            {
                code = ResponseCode.Win;
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
