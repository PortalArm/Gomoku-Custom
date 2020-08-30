using Gomoku_Custom.Game.Rules;
using Gomoku_Custom.Game.Strategies;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            GomokuPlayer player1 = new GomokuPlayer(new RandomStrategy(), Team.Red, Guid.NewGuid());
            GomokuPlayer player2 = new GomokuPlayer(new RandomStrategy(), Team.Blue, Guid.NewGuid());

            ConsoleGomokuFormatter cgf = new ConsoleGomokuFormatter(ConsoleGomokuFormatter.DefaultChars);
            GomokuGame gg = new GomokuGame(new MockRule());
            GameData data = gg.StartGame(Team.Blue);
            while (gg.State != GameState.GameEnded)
            {
                Console.SetCursorPosition(0,0);
                Console.WriteLine(cgf.RenderAsString(data));
                Console.WriteLine(data.Code);
                Console.WriteLine("Turn {0}. Enter coordinates as {1} player: ", gg.TurnNumber, gg.PlayerTurn);
                //int[] inputs =
                //Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                //.Select(v => int.Parse(v) - 1).ToArray();
                //Task.Delay(500).Wait();
                Point move = gg.PlayerTurn == player1.Team ? player1.ProposeMove(data) : player2.ProposeMove(data);
                data = gg.TryMakeMove(gg.PlayerTurn, move/*new Point(inputs[0], inputs[1])*/);
            }
            Console.WriteLine("Game ended. Press enter to see final move.");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine(cgf.RenderAsString(data));
            Console.WriteLine("{0} Lost! (Point {1})", data.NextPlayer, data.Updated);
        }
    }
}
