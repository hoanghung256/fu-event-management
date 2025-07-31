using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace FUEM.Web.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("ReceiveMessage", "System", $"You have joined the group {groupName}.");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("ReceiveMessage", "System", $"You have left the group {groupName}.");
        }

        public override async Task OnConnectedAsync()
        {
            // get userId authenticated
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(!string.IsNullOrEmpty(userId))
            {
                // Add user to a group based on their role or other criteria
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                Console.WriteLine($"Client {Context.ConnectionId} (User {userId} connected and added to group {userId}.)");
            }
            else
            {
                Console.WriteLine($"Client {Context.ConnectionId} (Anonymous) connected");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exeption)
        {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
                Console.WriteLine($"Client {Context.ConnectionId} (User {userId}) disconnected.");
            }
            else
            {
                Console.WriteLine($"Client {Context.ConnectionId} (Anonymous) disconnected.");
            }
            await base.OnDisconnectedAsync(exeption);
        }
    }
}
