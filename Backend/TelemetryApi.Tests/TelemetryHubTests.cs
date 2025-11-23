using Microsoft.AspNetCore.SignalR;
using Moq;
using TelemetryApi.Services;
using Xunit;

namespace TelemetryApi.Tests;

public class TelemetryHubTests
{
    [Fact]
    public async Task OnConnected_AddsClientToAllGroup()
    {
        var groupManager = new Mock<IGroupManager>();
        var context = new Mock<HubCallerContext>();
        context.SetupGet(c => c.ConnectionId).Returns("connection-1");

        var hub = new TelemetryHub
        {
            Groups = groupManager.Object,
            Context = context.Object
        };

        await hub.OnConnectedAsync();

        groupManager.Verify(g => g.AddToGroupAsync("connection-1", "all", default), Times.Once);
    }

    [Fact]
    public async Task JoinSector_AddsClientToSectorGroup()
    {
        var groupManager = new Mock<IGroupManager>();
        var context = new Mock<HubCallerContext>();
        context.SetupGet(c => c.ConnectionId).Returns("connection-2");

        var hub = new TelemetryHub
        {
            Groups = groupManager.Object,
            Context = context.Object
        };

        await hub.JoinSector(2);

        groupManager.Verify(g => g.AddToGroupAsync("connection-2", "sector.2", default), Times.Once);
    }

    [Fact]
    public async Task LeaveSector_RemovesClientFromSectorGroup()
    {
        var groupManager = new Mock<IGroupManager>();
        var context = new Mock<HubCallerContext>();
        context.SetupGet(c => c.ConnectionId).Returns("connection-3");

        var hub = new TelemetryHub
        {
            Groups = groupManager.Object,
            Context = context.Object
        };

        await hub.LeaveSector(3);

        groupManager.Verify(g => g.RemoveFromGroupAsync("connection-3", "sector.3", default), Times.Once);
    }
}
