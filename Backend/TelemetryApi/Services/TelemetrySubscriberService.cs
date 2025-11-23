using System.Text;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TelemetryApi.Services
{
    public class TelemetrySubscriberService : BackgroundService
    {
        private readonly ILogger<TelemetrySubscriberService> _logger;
        private readonly IHubContext<TelemetryHub> _hub;
        private readonly string _bindingKey;
        private readonly RabbitMqSettings _settings;

        public TelemetrySubscriberService(
            ILogger<TelemetrySubscriberService> logger,
            IHubContext<TelemetryHub> hub,
            string bindingKey,
            IOptions<RabbitMqSettings> settings)
        {
            _logger = logger;
            _hub = hub;
            _bindingKey = bindingKey;
            _settings = settings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = _settings.HostName };

            await using var connection = await ConnectWithRetry(factory, stoppingToken);
            await using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: _settings.ExchangeName, type: ExchangeType.Topic, cancellationToken: stoppingToken);

            var queueName = $"car-data-subscriber-{Guid.NewGuid()}";
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);

            await channel.QueueBindAsync(queue: queueName, exchange: _settings.ExchangeName, routingKey: _bindingKey, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation("[x] ({BindingKey}) {Message}", _bindingKey, message);

                await _hub.Clients.Group("all").SendAsync("telemetry", message, stoppingToken);

                if (_bindingKey.StartsWith("sector.", StringComparison.Ordinal) && _bindingKey != "sector.*")
                {
                    await _hub.Clients.Group(_bindingKey).SendAsync("telemetry", message, stoppingToken);
                }
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer, cancellationToken: stoppingToken);

            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Telemetry subscriber stopping");
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
