using F1Telemetry.Services;
using TelemetryApi.Services;
using Microsoft.Extensions.Options;

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

// Bind RabbitMQ settings from configuration/env vars (RabbitMq__HostName, etc.).
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));

// Generates fake telemetry and publishes it to RabbitMQ.
builder.Services.AddSingleton<TelemetryDataGenerator>();
builder.Services.AddHostedService<TelemetryPublisherService>();

// Create subscribers for all sectors and one for each individual sector.
builder.Services.AddSingleton<IHostedService>(p => new TelemetrySubscriberService(
    p.GetRequiredService<ILogger<TelemetrySubscriberService>>(),
    p.GetRequiredService<Microsoft.AspNetCore.SignalR.IHubContext<TelemetryHub>>(),
    "sector.*",
    p.GetRequiredService<IOptions<RabbitMqSettings>>()));
builder.Services.AddSingleton<IHostedService>(p => new TelemetrySubscriberService(
    p.GetRequiredService<ILogger<TelemetrySubscriberService>>(),
    p.GetRequiredService<Microsoft.AspNetCore.SignalR.IHubContext<TelemetryHub>>(),
    "sector.1",
    p.GetRequiredService<IOptions<RabbitMqSettings>>()));
builder.Services.AddSingleton<IHostedService>(p => new TelemetrySubscriberService(
    p.GetRequiredService<ILogger<TelemetrySubscriberService>>(),
    p.GetRequiredService<Microsoft.AspNetCore.SignalR.IHubContext<TelemetryHub>>(),
    "sector.2",
    p.GetRequiredService<IOptions<RabbitMqSettings>>()));
builder.Services.AddSingleton<IHostedService>(p => new TelemetrySubscriberService(
    p.GetRequiredService<ILogger<TelemetrySubscriberService>>(),
    p.GetRequiredService<Microsoft.AspNetCore.SignalR.IHubContext<TelemetryHub>>(),
    "sector.3",
    p.GetRequiredService<IOptions<RabbitMqSettings>>()));

var app = builder.Build();

app.UseCors();

// SignalR hub that relays telemetry to clients.
app.MapHub<TelemetryHub>("/hubs/telemetry");

// Print a simple startup banner so you know where to point the frontend.
app.Lifetime.ApplicationStarted.Register(() =>
{
    try { Console.Clear(); } catch { /* ignore if console not available */ }
    Console.WriteLine("==================================================");
    Console.WriteLine("Frontend: http://localhost:3000");
    Console.WriteLine("SignalR hub: http://localhost:5010/hubs/telemetry");
    Console.WriteLine("==================================================");
});

app.Run();
