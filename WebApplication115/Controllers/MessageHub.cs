using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;

    public override Task OnConnectedAsync()
    {
        Console.WriteLine("Connection established: " + Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        Console.WriteLine("Connection disconnected: " + Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    public async Task SendNotification(string message)
    {
        _logger.LogInformation("Sending notification: {Message}", message);

        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}
