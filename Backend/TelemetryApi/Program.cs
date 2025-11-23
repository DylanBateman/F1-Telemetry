using F1Telemetry.Services;
using TelemetryApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Core API + SignalR endpoints and permissive CORS for local dev/frontends.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials());
});

// Generates fake telemetry and publishes it to RabbitMQ.
builder.Services.AddSingleton<TelemetryDataGenerator>();
builder.Services.AddHostedService<TelemetryPublisherService>();

// Create subscribers for all sectors and one for each individual sector.
builder.Services.AddSingleton<IHostedService>(p => new TelemetrySubscriberService(
    p.GetRequiredService<ILogger<TelemetrySubscriberService>>(),
    p.GetRequiredService<Microsoft.AspNetCore.SignalR.IHubContext<TelemetryHub>>(),
    "sector.*"));
builder.Services.AddSingleton<IHostedService>(p => new TelemetrySubscriberService(
    p.GetRequiredService<ILogger<TelemetrySubscriberService>>(),
    p.GetRequiredService<Microsoft.AspNetCore.SignalR.IHubContext<TelemetryHub>>(),
    "sector.1"));
builder.Services.AddSingleton<IHostedService>(p => new TelemetrySubscriberService(
    p.GetRequiredService<ILogger<TelemetrySubscriberService>>(),
    p.GetRequiredService<Microsoft.AspNetCore.SignalR.IHubContext<TelemetryHub>>(),
    "sector.2"));
builder.Services.AddSingleton<IHostedService>(p => new TelemetrySubscriberService(
    p.GetRequiredService<ILogger<TelemetrySubscriberService>>(),
    p.GetRequiredService<Microsoft.AspNetCore.SignalR.IHubContext<TelemetryHub>>(),
    "sector.3"));

var app = builder.Build();

app.UseCors();

// SignalR hub that relays telemetry to clients.
app.MapHub<TelemetryHub>("/hubs/telemetry");

app.Run();
