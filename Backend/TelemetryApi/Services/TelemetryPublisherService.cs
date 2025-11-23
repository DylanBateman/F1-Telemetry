using System.Text;
using System.Text.Json;
using F1Telemetry.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace TelemetryApi.Services
{
    public class TelemetryPublisherService : BackgroundService
    {
        private readonly TelemetryDataGenerator _generator;
        private readonly ILogger<TelemetryPublisherService> _logger;

        public TelemetryPublisherService(TelemetryDataGenerator generator, ILogger<TelemetryPublisherService> logger)
        {
            _generator = generator;
            _logger = logger;
        }

        // generate telemtry data every 2 seconds and send the RabbitMQ queues
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "car-data", type: ExchangeType.Topic, cancellationToken: stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                var telemetry = _generator.GenerateTelemetryData();

                var jsonTelemetry = JsonSerializer.Serialize(telemetry);
                var body = Encoding.UTF8.GetBytes(jsonTelemetry);
                var routingKey = $"sector.{telemetry.Sector}";

                await channel.BasicPublishAsync(
                    exchange: "car-data",
                    routingKey: routingKey,
                    body: body,
                    mandatory: false,
                    cancellationToken: stoppingToken);

                _logger.LogInformation("[x] Sent telemetry (routingKey={RoutingKey}): {Json}", routingKey, jsonTelemetry);

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}
