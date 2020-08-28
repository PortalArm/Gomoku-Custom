using System;
using System.IO;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace Gomoku_Custom.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new GomokuServer(TimeSpan.FromMinutes(1)))
                server.Listen();
        }
    }
}
