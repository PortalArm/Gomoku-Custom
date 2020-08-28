using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Server
{
    //public class GomokuServer2 : IDisposable
    //{
    //    private readonly TcpListener _listener;
    //    private readonly BinaryFormatter _bf;
    //    private readonly Guid[] _players;
    //    private int _playerTurn;
    //    public GomokuServer(byte[] ip, int port = 5010)// "http://localhost:5010/game/")
    //    {
    //        _bf = new BinaryFormatter();
    //        _players = new Guid[2];
    //        _listener = new TcpListener(new IPAddress(ip), port);
    //        _playerTurn = 0;
    //    }

    //    public void Dispose()
    //    {
    //        ((IDisposable)_listener).Dispose();
    //    }

    //    public void Listen()
    //    {
    //        Console.WriteLine("Listening started. Press enter to stop.");
    //        _listener.Start();
    //        var result = _listener.BeginAcceptSocket(new AsyncCallback(ProcessRequest), null);
    //        Console.ReadLine();
    //        _listener.Stop();
    //    }

    //    private InitData IssueToken()
    //    {
    //        if (_players[0] != Guid.Empty && _players[1] != Guid.Empty)
    //            return new InitData { Side = Team.Unknown, Code = ResponseCode.AccessDenied };
    //        if (_players[0] == Guid.Empty)
    //            return new InitData { ClientId = _players[0] = Guid.NewGuid(), Side = Team.Blue, Code = ResponseCode.OK };

    //        return new InitData { ClientId = _players[1] = Guid.NewGuid(), Side = Team.Red, Code = ResponseCode.OK };

    //    }
    //    private void ProcessRequest(IAsyncResult result)
    //    {
    //        DateTime requestTime = DateTime.Now;
    //        var context = _listener.EndAcceptSocket(result);
    //        _listener.BeginAcceptSocket(ProcessRequest, null);
    //        var request = context.Send(;

    //        //Получение данных с клиента
    //        object userData = null;
    //        if (request.InputStream is not null and { Length: > 0 })
    //            userData = _bf.Deserialize(request.InputStream);

    //        var response = context.Response;
    //        switch (userData)
    //        {
    //            //Если первый запрос от клиента N
    //            case null:
    //                InitData id = IssueToken();
    //                _bf.Serialize(response.OutputStream, id);
    //                break;

    //            default:
    //                Console.WriteLine("Unrecognized content: {0}, {1}", nameof(userData), userData.GetType());
    //                break;
    //        }
    //        response.OutputStream.Close();
    //        //Console.WriteLine(((Point)userData).X);
    //        Console.WriteLine(requestTime);
    //    }
    //}
}
