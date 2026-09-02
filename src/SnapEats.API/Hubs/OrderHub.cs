namespace SnapEats.API.Hubs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Serilog;

[AllowAnonymous]
public class OrderHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        Log.Information("SignalR Connection established. ConnectionId: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Log.Information("SignalR Connection disconnected. ConnectionId: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Allows a client to subscribe to real-time updates for a specific order.
    /// </summary>
    public async Task JoinOrderGroup(int orderId)
    {
        var groupName = $"Order:{orderId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        Log.Information("ConnectionId {ConnectionId} joined group '{GroupName}'", Context.ConnectionId, groupName);
    }

    /// <summary>
    /// Allows a client to unsubscribe from real-time updates for a specific order.
    /// </summary>
    public async Task LeaveOrderGroup(int orderId)
    {
        var groupName = $"Order:{orderId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        Log.Information("ConnectionId {ConnectionId} left group '{GroupName}'", Context.ConnectionId, groupName);
    }
}
