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
        Point Predict(GameData gd);
    }
}
