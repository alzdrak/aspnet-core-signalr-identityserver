using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace server.Hubs
{
    [Authorize]
    public class ChatHub: Hub
    {
        public async Task SendMessage(string message)
        {
            var username = Context.User.Identity.Name;

            //inform other clients
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }

        public override async Task OnConnectedAsync()
        {
            //get user thats connecting
            var connectionId = Context.ConnectionId;

            //add user to a group
            await Groups.AddToGroupAsync(connectionId, "Chatroom");

            //send message to user
            await Clients.Client(connectionId).SendAsync("UserJoined");

            //informe to all other users
            await Clients.AllExcept(connectionId).SendAsync("NewUser");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //get user disconnecting
            var connectionId = Context.ConnectionId;

            //remove user from group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Chatroom");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
