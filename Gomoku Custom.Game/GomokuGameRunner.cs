using Gomoku_Custom.Game.Rules;
using Gomoku_Custom.Game.Strategies;
using Gomoku_Custom.Shared;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game
{
    public class GomokuGameRunner
    {
        public GomokuPlayer PlayerBlue { get; private set; }
        public GomokuPlayer PlayerRed { get; private set; }
        public GomokuGame Game { get; private set; }
        public GomokuGameRunner()
        {
            Console.WriteLine("Runner created!");
        }
        //public void Init(Team team, IStrategy strategy, Guid id)
        //{
        //    var newPlayer = new GomokuPlayer(strategy, team, id);
        //    if (team == Team.Red)
        //        PlayerRed = newPlayer;
        //    else
        //    if (team == Team.Blue)
        //        PlayerBlue = newPlayer;
        //    if (PlayerRed != null && PlayerBlue != null)
        //        OnPlayersConnected?.Invoke();
        //}

        public void Init(Team team, Type strategy, Guid id)
        {
            var newPlayer = new GomokuPlayer(StrategyFactory.Create(strategy, Game, team), team, id);
            if (team == Team.Red)
                PlayerRed = newPlayer;
            else
            if (team == Team.Blue)
                PlayerBlue = newPlayer;

            if (PlayerRed != null && PlayerBlue != null)
                OnPlayersConnected?.Invoke();
        }

        public async Task RunAuto()
        {
            var data = Game.StartGame(Team.Blue);
            OnGameStarted?.Invoke(data);
            Console.WriteLine("Runner started an auto game!");

            while (Game.State != GameState.GameEnded)
            {
                Point move;
                if (Game.PlayerTurn == Team.Blue)
                {
                    move = PlayerBlue.ProposeMove(data);
                    PlayerRed.UpdateState(data);
                } else
                {
                    move = PlayerRed.ProposeMove(data);
                    PlayerBlue.UpdateState(data);
                }
                data = Game.TryMakeMove(Game.PlayerTurn, move);
                if (data.Code == ResponseCode.OK)
                    await OnMoveCompleted?.Invoke(data);
                if (Game.State == GameState.GameEnded)
                {
                    await OnMoveCompleted?.Invoke(data);
                    await OnGameEnded?.Invoke(data);
                    return;
                }
                await Task.Delay(500);

            }
            //if (data.Code == ResponseCode.Draw)
            //    Console.WriteLine("Draw!");
            //else
            //    Console.WriteLine("{0} Lost! (Point {1})", data.NextPlayer, data.Updated);
        }
        //public void ModifyWith(Type strategy, Team team) {
        //    GomokuPlayer player = team == Team.Red ? PlayerRed : PlayerBlue;    
        //    if(player is null) 
        //        return;

        //    player.UpdateStrategy(StrategyFactory.Create(strategy, Game, player.Team));
        //}
        public void InitGame(int fieldSize, int winLength = 5)
        {
            Game = new GomokuGame(new MockRule(), Team.Blue, fieldSize, winLength);
        }
        private void UpdateOpponentState(GomokuPlayer player, GameData data)
        {
            if (player == PlayerBlue)
                PlayerRed.UpdateState(data);
            if (player == PlayerRed)
                PlayerBlue.UpdateState(data);

        }
        public async Task Move(string id, int row, int col)
        {
            if (!EnsureExists(id, out var player))
                return;
            var data = Game.TryMakeMove(player.Team, new Point(col, row));
            UpdateOpponentState(player, data);

            if (data.Code != ResponseCode.OK && Game.State != GameState.GameEnded)
                return;

            await OnMoveCompleted?.Invoke(data);
            if (Game.State == GameState.GameEnded)
                await OnGameEnded?.Invoke(data);
            //Move(data);
        }
        public void Move(GameData gd)
        {
            var player = GetPlayerOf(gd.NextPlayer);
            var move = player.ProposeMove(gd);
            var data = Game.TryMakeMove(gd.NextPlayer, move);
            UpdateOpponentState(player, data);

            if (data.Code != ResponseCode.OK && Game.State != GameState.GameEnded)
                return;

            OnMoveCompleted?.Invoke(data);
            if (Game.State == GameState.GameEnded)
                OnGameEnded?.Invoke(data);
        }
        public delegate Task AsyncEventHandler<T>(T args);
        public delegate Task AsyncEventHandler();
        public event AsyncEventHandler OnPlayersConnected;
        public event AsyncEventHandler<GameData> OnGameStarted;
        public event AsyncEventHandler<GameData> OnMoveCompleted;
        public event AsyncEventHandler<GameData> OnGameEnded;
        public void Start()
        {
            var data = Game.StartGame(Team.Blue);
            OnGameStarted?.Invoke(data);
            Console.WriteLine("Runner started the game!");
        }
        public GomokuPlayer GetPlayerOf(Team team)
        {
            return team switch
            {
                Team.Red => PlayerRed,
                Team.Blue => PlayerBlue,
                _ => throw new ArgumentOutOfRangeException(nameof(team))
            };
        }
        private bool EnsureExists(string id, out GomokuPlayer player)
        {
            Guid.TryParse(id, out Guid guid);
            if (PlayerBlue != null && PlayerBlue.Id == guid)
            {
                player = PlayerBlue;
                return true;
            }
            if (PlayerRed != null && PlayerRed.Id == guid)
            {
                player = PlayerRed;
                return true;
            }
            player = null;
            return false;
        }
        //public enum GameState
        //{
        //    AwaitingConnections,
        //    GameStarted,
        //    GameEnded,
        //    WaitingForBlue,
        //    WaitingForRed,
        //    AwaitingStart
        //}
        private static readonly string[] StateResponses = {
            "Waiting players ...",
            "Game started",
            "Game ended",
            "Waiting for Blue's turn",
            "Waiting for Red's turn",
            "Waiting for start...",
        };
        public string GetStateInfo() => StateResponses[(int)Game.State];
        public GameState GetState() => Game.State;
        public Team GetTeamOf(string id)
        {
            if (EnsureExists(id, out var player))
                return player.Team;
            return Team.None;
        }

        public Team[][] GetCurrentField()
        {
            //?
            if (Game == null ||
                Game.State == GameState.GameEnded ||
                Game.State == GameState.AwaitingConnections ||
                Game.State == GameState.AwaitingStart)
                return null;
            return Game.Field;
        }
    }
}
