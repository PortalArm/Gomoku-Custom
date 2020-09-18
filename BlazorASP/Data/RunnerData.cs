using Gomoku_Custom.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gomoku_Custom.Blazor.Data
{
    public class RunnerData
    {
        public HostPrefs Prefs { get; set; }
        public int Joined { get; set; }
        public bool IsStarted { get; set; }
        public GomokuGameRunner Runner { get; set; }
    }
}
