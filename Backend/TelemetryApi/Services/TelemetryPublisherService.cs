using System.Text;
using System.Text.Json;
using F1Telemetry.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TelemetryApi.Services
{
    public class TelemetryPublisherService : BackgroundService
    {
        private readonly TelemetryDataGenerator _generator;
        private readonly ILogger<TelemetryPublisherService> _logger;
        private readonly RabbitMqSettings _settings;

        public TelemetryPublisherService(
            TelemetryDataGenerator generator,
            ILogger<TelemetryPublisherService> logger,
            IOptions<RabbitMqSettings> settings)
        {
            _generator = generator;
            _logger = logger;
            _settings = settings.Value;
        }

        // generate telemtry data every 2 seconds and send the RabbitMQ queues
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = _settings.HostName };

            await using var connection = await ConnectWithRetry(factory, stoppingToken);
            await using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: _settings.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                var telemetry = _generator.GenerateTelemetryData();

                var jsonTelemetry = JsonSerializer.Serialize(telemetry);
                var body = Encoding.UTF8.GetBytes(jsonTelemetry);
                var routingKey = $"sector.{telemetry.Sector}";

                await channel.BasicPublishAsync(
                    exchange: _settings.ExchangeName,
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

        private static async Task<IConnection> ConnectWithRetry(ConnectionFactory factory, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    return await factory.CreateConnectionAsync();
                }
                catch
                {
                    await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                }
            }

            throw new OperationCanceledException(stoppingToken);
        }
    }
}
