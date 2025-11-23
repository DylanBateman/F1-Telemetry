using Microsoft.AspNetCore.SignalR;

namespace TelemetryApi.Services;

public class TelemetryHub : Hub
{
    private const string AllGroup = "all";

    // every client joins the all data group on connect so they receive all telemetry.
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, AllGroup);
        await base.OnConnectedAsync();
    }

    // clients can opt into sector-specific streams.
    public Task JoinSector(int sector) => Groups.AddToGroupAsync(Context.ConnectionId, SectorGroup(sector));

    public Task LeaveSector(int sector) => Groups.RemoveFromGroupAsync(Context.ConnectionId, SectorGroup(sector));

    private static string SectorGroup(int sector) => $"sector.{sector}";
}
