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
        private IStrategy _strategy;
        public Team Team { get; set; }
        private Guid _id;
        public GomokuPlayer(IStrategy strategy, Team team, Guid id)
        {
            _strategy = strategy;
            Team = team;
            _id = id;
        }
        public void UpdateStrategy(IStrategy strategy) =>
            _strategy = strategy;
        public Point ProposeMove(GameData gd) {
            return _strategy.UpdateAndPredict(gd);
        }
        public void UpdateState(GameData gd) {
            _strategy.UpdateState(gd);
        }
    }
}
