using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Rtm.Hubs
{
    public class GameHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var connectionId = this.Context.ConnectionId;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

        }
    }
}