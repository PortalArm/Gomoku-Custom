using Gomoku_Custom.Shared;
using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Gomoku_Custom.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new GomokuServer())
                server.Listen();
        }
    }

    class GomokuServer : IDisposable
    {
        private readonly HttpListener _listener;
        private readonly BinaryFormatter _bf;
        private readonly Guid[] _players;
        private int _playerTurn;
        public GomokuServer(string baseUri = "http://localhost:5010/game/")
        {
            _bf = new BinaryFormatter();
            _players = new Guid[2];
            _listener = new HttpListener();
            _listener.Prefixes.Add(baseUri);
            _playerTurn = 0;
        }

        public void Dispose()
        {
            ((IDisposable)_listener).Dispose();
        }

        public void Listen()
        {
            if (_listener.IsListening)
                throw new NotSupportedException();
            Console.WriteLine("Listening started. Press enter to stop.");
            _listener.Start();
            var result = _listener.BeginGetContext(new AsyncCallback(ProcessRequest), null);
            Console.ReadLine();
            _listener.Stop();
        }

        private InitData IssueToken() {
            if (_players[0] != Guid.Empty && _players[1] != Guid.Empty)
                return new InitData { Side = Team.Unknown, Code = ResponseCode.AccessDenied };
            if (_players[0] == Guid.Empty)
                return new InitData {ClientId = _players[0] = Guid.NewGuid(), Side = Team.Blue, Code = ResponseCode.OK };
            
            return new InitData {ClientId = _players[1] = Guid.NewGuid(), Side = Team.Red, Code = ResponseCode.OK };

        }
        private void ProcessRequest(IAsyncResult result)
        {
            DateTime requestTime = DateTime.Now;
            var context = _listener.EndGetContext(result);
            _listener.BeginGetContext(ProcessRequest, null);
            var request = context.Request;

            object userData = null;
            if (request.InputStream is not null and { Length : >0 })
                userData = _bf.Deserialize(request.InputStream);

            var response = context.Response;

            if (userData is null)
            {
                InitData id = IssueToken();
                _bf.Serialize(response.OutputStream, id);
                response.OutputStream.Close();
            }
            //Console.WriteLine(((Point)userData).X);
            Console.WriteLine(requestTime);
        }
    }
}
