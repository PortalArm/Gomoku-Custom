using Gomoku_Custom.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gomoku_Custom.Blazor.Data
{
    public class GameService
    {
        private static GameService _instance;
        public static GameService Instance => _instance ??= new GameService();
        private ConcurrentDictionary<string, RunnerData> _runners;
        private GameService()
        {
            //timer = new Timer(TimerRoutine, null, 5000, 5000);
            _runners = new ConcurrentDictionary<string, RunnerData>();
        }
        
        public bool Contains(string key) => _runners.ContainsKey(key);
        public bool Delete(string key) => _runners.TryRemove(key, out _);
        public RunnerData Get(string key) => _runners[key];
        public RunnerData Set(string key, RunnerData runnerData) => _runners[key] = runnerData;


        //public void TimerRoutine(object obj) {
        //    OnStateChanged?.Invoke(this, new EventArgs());
        //}
        //public event EventHandler OnStateChanged;
        //private static Timer timer;
        
    }
}
