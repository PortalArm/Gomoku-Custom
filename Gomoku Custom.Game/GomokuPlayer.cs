using Gomoku_Custom.Game.Strategies;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game
{
    public class GomokuPlayer
    {
        public IStrategy Strategy { get; private set; }
        public Team Team { get; private set; }
        public Guid Id { get; private set; }
        // get rid of team, use strategy's team instead
        public GomokuPlayer(IStrategy strategy, Team team, Guid id)
        {
            Strategy = strategy;
            Team = team;
            Id = id;
        }
        public void UpdateStrategy(IStrategy strategy) =>
            Strategy = strategy;
        public Point ProposeMove(GameData gd) {
            return Strategy.UpdateAndPredict(gd);
        }
        public void UpdateState(GameData gd) {
            Strategy.UpdateState(gd);
        }
    }
}
