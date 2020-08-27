﻿using Gomoku_Custom.Shared;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Gomoku_Custom.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter to send");
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

    public class GomokuClient : IDisposable
    {
        private readonly HttpClient _client;
        private readonly BinaryFormatter _bf; // не уверен
        //private readonly HttpClient _listener;
        private InitData _init;
        public GomokuClient(string baseAddress = "http://localhost:5010/game/")
        {
            _client = new HttpClient() { BaseAddress = new Uri(baseAddress) };
            _bf = new BinaryFormatter();
        }

        public void Dispose() =>
            ((IDisposable)_client).Dispose();

        //public async Task Start()
        //{
        //    var request = new HttpRequestMessage() { Method = HttpMethod.Get };
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        object obj = new Point(3, 7);
        //        _bf.Serialize(ms, obj);
        //        request.Content = new ByteArrayContent(ms.ToArray());
        //    }
        //    var response = await _client.SendAsync(request);
        //    string str = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine(str);
        //}
        public async Task EstablishConnection()
        {
            var request = new HttpRequestMessage() { Method = HttpMethod.Get };
            var response = await _client.SendAsync(request);
            Stream stream = await response.Content.ReadAsStreamAsync();
            var responseObject = _bf.Deserialize(stream);

            _ = responseObject switch { 
                InitData id => _init = id,
                _ => throw new InvalidOperationException()
            };
            Console.WriteLine("Got my team: {0} and client id: {1}", _init.Side, _init.ClientId);
        }
        public async Task Send(object obj)
        {
            if (_init.ClientId == Guid.Empty)
                throw new NotSupportedException();

            var request = new HttpRequestMessage() { Method = HttpMethod.Get };
            using (MemoryStream ms = new MemoryStream())
            {
                _bf.Serialize(ms, obj);
                request.Content = new ByteArrayContent(ms.ToArray());
            }
            var response = await _client.SendAsync(request);
            string str = await response.Content.ReadAsStringAsync();
            Console.WriteLine(str);
        }
    }
}
