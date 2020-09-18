using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game.Strategies
{
    public interface IStrategy
    {
        Point UpdateAndPredict(GameData gd);
        void UpdateState(GameData gd);
        bool IsHuman { get; }
    }
}
