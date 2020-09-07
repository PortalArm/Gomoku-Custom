using Microsoft.AspNetCore.SignalR;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorASP.Connectivity
{
    public class GomokuHub : Hub
    {
        public async Task SendMessage(string user, string message) {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
