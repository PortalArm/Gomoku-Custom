using Gomoku_Custom.Game.Formatters;
using Gomoku_Custom.Game.Rules;
using Gomoku_Custom.Game.Strategies;
using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gomoku_Custom.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 0;
            for (int test = 0; test < 50; ++test)
            {
                List<Point> history = new List<Point>();
                File.WriteAllText("output.txt", "");
                GomokuPlayer player1 = new GomokuPlayer(null, Team.Red, Guid.NewGuid());
                GomokuPlayer player2 = new GomokuPlayer(new ConsoleHumanStrategy(), Team.Blue, Guid.NewGuid());
                //GomokuPlayer player2 = new GomokuPlayer(null, Team.Blue, Guid.NewGuid());

                ConsoleGomokuFormatter cgf = new ConsoleGomokuFormatter(ConsoleGomokuFormatter.DefaultChars);
                GomokuGame gg = new GomokuGame(new MockRule(), fieldSize: 19, winLength: 5);
                //GomokuGame gg = new GomokuGame(new MockRule(), fieldSize: 3, winLength: 3);
                GameData data = gg.StartGame(Team.Blue);
                player1.UpdateStrategy(new NaiveStrategy(gg.FieldSize, player1.Team, gg.WinLength, maxDepth: 4));
                //player2.UpdateStrategy(new NaiveStrategy(gg.FieldSize, player2.Team, gg.WinLength, 6));
                while (gg.State != GameState.GameEnded)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(cgf.RenderAsString(data));
                    Console.WriteLine(data.Code);
                    Console.WriteLine("Turn {0}. Enter coordinates as {1} player: ", gg.TurnNumber, gg.PlayerTurn);
                    //int[] inputs =
                    //Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    //.Select(v => int.Parse(v) - 1).ToArray();
                    //Task.Delay(500).Wait();
                    Point move;
                    if (gg.PlayerTurn == player1.Team)
                    {
                        move = player1.ProposeMove(data);
                        player2.UpdateState(data);
                    } else
                    {
                        move = player2.ProposeMove(data);
                        player1.UpdateState(data);
                    }
                    //Point move = gg.PlayerTurn == player1.Team ? player1.ProposeMove(data) : player2.ProposeMove(data);
                    Console.WriteLine("Trying to move ({0}, {1})", gg.PlayerTurn, move);
                    data = gg.TryMakeMove(gg.PlayerTurn, move);
                    if (data.Code == ResponseCode.OK)
                        history.Add(move);
                    //Console.ReadLine();
                }
                Console.WriteLine("Game ended. Press enter to see final move.");
                Console.ReadLine();

                Console.SetCursorPosition(0, 0);/* Console.Clear();*/
                Console.WriteLine(cgf.RenderAsString(data));
                if (data.Code == ResponseCode.Draw)
                {
                    count++;
                    Console.WriteLine("Draw!");
                } else
                    Console.WriteLine("{0} Lost! (Point {1})", data.NextPlayer, data.Updated);
                Console.WriteLine(string.Join(", ", history));
                Console.ReadKey();
            }
            Console.Clear();
            Console.WriteLine(count);
        }

    }
}
