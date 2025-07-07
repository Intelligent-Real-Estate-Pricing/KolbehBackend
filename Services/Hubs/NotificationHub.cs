using Microsoft.AspNetCore.SignalR;
using Services.Services.NotifHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IConnectionMappingService _connectionService;

        public NotificationHub(IConnectionMappingService connectionService)
        {
            _connectionService = connectionService;
        }

        public override async Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier ?? Context.ConnectionId;
            _connectionService.Add(userId, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.UserIdentifier ?? Context.ConnectionId;
            _connectionService.Remove(userId, Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }

}
