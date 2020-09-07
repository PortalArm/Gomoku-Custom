using Gomoku_Custom.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gomoku_Custom.Blazor.Data
{
    public class GameService
    {
        private static GameService _instance;
        public static GameService Instance => _instance ??= new GameService();
        private Dictionary<string, GomokuGameRunner> _runners;
        private GameService()
        {
            _runners = new Dictionary<string, GomokuGameRunner>();
        }
        public bool Contains(string key) => _runners.ContainsKey(key);
        public GomokuGameRunner Get(string key) => _runners[key];
        public GomokuGameRunner Set(string key, GomokuGameRunner runner) => _runners[key] = runner;
    }
}
