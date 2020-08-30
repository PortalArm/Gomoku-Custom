using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game.Strategies
{
    public class RandomStrategy : IStrategy
    {
        private static readonly Random _rand = new Random();
        public Point Predict(GameData gd)
        {
            int bound = gd.Field.GetLength(0);
            return new Point(_rand.Next(bound), _rand.Next(bound));
        }
    }
}
