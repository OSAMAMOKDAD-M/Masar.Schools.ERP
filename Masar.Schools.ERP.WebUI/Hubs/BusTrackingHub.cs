using Microsoft.AspNetCore.SignalR;

namespace Masar.Schools.ERP.WebUI.Hubs;

/// <summary>
/// SignalR Hub for real-time bus tracking updates
/// </summary>
public class BusTrackingHub : Hub
{
    /// <summary>
    /// Join a specific bus tracking group
    /// </summary>
    public async Task JoinBusGroup(string busId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Bus_{busId}");
        await Clients.Caller.SendAsync("JoinedBusGroup", busId);
    }

    /// <summary>
    /// Leave a specific bus tracking group
    /// </summary>
    public async Task LeaveBusGroup(string busId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Bus_{busId}");
        await Clients.Caller.SendAsync("LeftBusGroup", busId);
    }

    /// <summary>
    /// Broadcast bus location update to all clients tracking this bus
    /// </summary>
    public async Task BroadcastBusLocation(string busId, object locationData)
    {
        await Clients.Group($"Bus_{busId}").SendAsync("BusLocationUpdate", busId, locationData);
    }

    /// <summary>
    /// Broadcast to all clients (for system-wide updates)
    /// </summary>
    public async Task BroadcastToAll(string message, object data)
    {
        await Clients.All.SendAsync(message, data);
    }

    /// <summary>
    /// Notify when a bus starts moving
    /// </summary>
    public async Task NotifyBusStarted(string busId, object busData)
    {
        await Clients.Group($"Bus_{busId}").SendAsync("BusStarted", busId, busData);
    }

    /// <summary>
    /// Notify when a bus stops
    /// </summary>
    public async Task NotifyBusStopped(string busId, object busData)
    {
        await Clients.Group($"Bus_{busId}").SendAsync("BusStopped", busId, busData);
    }

    /// <summary>
    /// Notify when a bus goes off route
    /// </summary>
    public async Task NotifyBusOffRoute(string busId, object alertData)
    {
        await Clients.Group($"Bus_{busId}").SendAsync("BusOffRoute", busId, alertData);
    }

    /// <summary>
    /// Notify when a bus is approaching a stop
    /// </summary>
    public async Task NotifyBusApproachingStop(string busId, string stopName, object stopData)
    {
        await Clients.Group($"Bus_{busId}").SendAsync("BusApproachingStop", busId, stopName, stopData);
    }

    /// <summary>
    /// Notify when a bus arrives at a stop
    /// </summary>
    public async Task NotifyBusArrivedAtStop(string busId, string stopName, object stopData)
    {
        await Clients.Group($"Bus_{busId}").SendAsync("BusArrivedAtStop", busId, stopName, stopData);
    }

    /// <summary>
    /// Send alert for speed violation
    /// </summary>
    public async Task NotifySpeedViolation(string busId, object alertData)
    {
        await Clients.Group($"Bus_{busId}").SendAsync("SpeedViolation", busId, alertData);
    }

    /// <summary>
    /// Send maintenance alert
    /// </summary>
    public async Task NotifyMaintenanceAlert(string busId, object alertData)
    {
        await Clients.Group($"Bus_{busId}").SendAsync("MaintenanceAlert", busId, alertData);
    }

    /// <summary>
    /// Connection management
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await Clients.Caller.SendAsync("Connected", "Connected to Bus Tracking Hub");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
