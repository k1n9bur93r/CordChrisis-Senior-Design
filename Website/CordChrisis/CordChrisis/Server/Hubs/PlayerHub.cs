using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CordChrisis.Server.Hubs
{
    public class PlayerHub : Hub
    {
        public async Task Hello(string data)
        {
            await Clients.All.SendAsync("Sup","Hey Homie");
        }
    }
}
