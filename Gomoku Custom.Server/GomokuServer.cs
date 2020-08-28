using Gomoku_Custom.Shared;
using System;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Gomoku_Custom.Server
{
    public class GomokuServer : IDisposable
    {
        private readonly HttpListener _listener;
        private readonly BinaryFormatter _bf;
        private readonly Guid[] _players;
        private TimeSpan _turnTime;
        private int _playerTurn;
        private bool IsGuidPresent(Guid guid) =>
            _players[0] == guid || _players[1] == guid;
        public GomokuServer(TimeSpan turnTime, string baseUri = "http://localhost:5010/game/")
        {
            _bf = new BinaryFormatter();
            _players = new Guid[2];
            Console.WriteLine(_players[0] == Guid.Empty);
            _listener = new HttpListener();
            _listener.Prefixes.Add(baseUri);
            _playerTurn = 0;
            _turnTime = turnTime;
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
            //Если места уже заняты
            if (!IsGuidPresent(Guid.Empty))
                return new InitData { Side = Team.Unknown, Code = ResponseCode.AccessDenied };
            //Первый законнектившийся
            if (_players[0] == Guid.Empty)
                return new InitData {ClientId = _players[0] = Guid.NewGuid(), Side = Team.Blue, Code = ResponseCode.OK };
            //Второй законнектившийся
            return new InitData {ClientId = _players[1] = Guid.NewGuid(), Side = Team.Red, Code = ResponseCode.OK };
        }
        private void ProcessRequest(IAsyncResult result)
        {
            DateTime requestTime = DateTime.Now;
            var context = _listener.EndGetContext(result);
            _listener.BeginGetContext(ProcessRequest, null);
            var request = context.Request;

            //Получение данных с клиента
            object userData = null;
            if (request.InputStream is not null and { Length : >0 })
                userData = _bf.Deserialize(request.InputStream);

            var response = context.Response;
            switch (userData)
            {
                //Если первый запрос от клиента N
                case null:
                    //Task.Delay(15000).Wait();
                    InitData id = IssueToken();
                    _bf.Serialize(response.OutputStream, id);
                    break;
                default:
                    Console.WriteLine("Unrecognized content: {0}, {1}", nameof(userData), userData.GetType());
                    break;
            }
            response.OutputStream.Close();
            //Console.WriteLine(((Point)userData).X);
            Console.WriteLine(requestTime);
        }
    }
}
