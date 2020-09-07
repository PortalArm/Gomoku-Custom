using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game.Strategies
{
    public class HumanStrategy : IStrategy
    {
        public Point UpdateAndPredict(GameData gd)
        {
            int[] inputs = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            return new Point(inputs[0],inputs[1]);
        }

        public void UpdateState(GameData gd)
        {
        }
    }
}
