using Gomoku_Custom.Shared;
using System;
using System.Threading.Tasks;

namespace Gomoku_Custom.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter to establish connection");
            Console.ReadLine();
            using (GomokuClient gc = new GomokuClient())
            {
                await gc.EstablishConnection();
                Console.ReadLine();
                await gc.Send(new Point(2, 1));
                Console.ReadLine();
                await gc.Send(new GameData() { Code = ResponseCode.OK });
                Console.ReadLine();
                await gc.Send(new MoveData() { Proposed = new Point(7, 8) });
            }
                Console.ReadLine();
        }
    }
}
