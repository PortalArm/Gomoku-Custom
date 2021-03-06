﻿using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gomoku_Custom.Game.Strategies
{
    public class ExternalStrategy : IStrategy
    {
        public bool IsHuman => true;

        private Func<GameData, Point> _action;
        public ExternalStrategy(Func<GameData, Point> action)
        {
            _action = action;
        }
        public Point UpdateAndPredict(GameData gd)
        {
            return _action?.Invoke(gd) ?? Point.Empty;
        }

        public void UpdateState(GameData gd)
        {
        }
    }
}
