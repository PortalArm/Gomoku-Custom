using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gomoku_Custom.Game.Strategies
{
    public class StrategyFactory
    {
        public static readonly Type[] PlayableStrategies = {
            typeof(NaiveStrategy),
            typeof(RandomStrategy),
            typeof(ExternalStrategy),
        };
        public static readonly Type[] AutoStrategies = {
            typeof(RandomStrategy),
            typeof(NaiveStrategy),
        };
        public static readonly Dictionary<Type, string> InfoForStrategies = new Dictionary<Type, string> {
            {typeof(NaiveStrategy) , "Base AI Algorithm"},
            {typeof(ExternalStrategy) , "Human"},
            {typeof(RandomStrategy) , "Computer with random moves"},
        };
        public static IStrategy Create(Type type, GomokuGame game, Team team)// GomokuPlayer player)
        {
            if (type == typeof(ExternalStrategy))
                return new ExternalStrategy(null);
            if (type == typeof(RandomStrategy))
                return new RandomStrategy();
            if (type == typeof(NaiveStrategy))
                return new NaiveStrategy(game.FieldSize, team, game.WinLength);
            throw new ArgumentException(nameof(type));
        }
    }
}
