using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using F1Telemetry.Services;
using RabbitMQ.Client;

class Publisher
{
    static async Task Main(string[] args)
    {
        var generator = new TelemetryDataGenerator();

        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: "car-data",
            type: ExchangeType.Topic
        );

        while (true)
        {
            var telemetry = generator.GenerateTelemetryData();

            var jsonTelemetry = JsonSerializer.Serialize(telemetry);
            var body = Encoding.UTF8.GetBytes(jsonTelemetry);

            var routingKey = $"sector.{telemetry.Sector}";

            await channel.BasicPublishAsync(
                exchange: "car-data",
                routingKey: routingKey,
                body: body
            );

            Console.WriteLine($"[x] Sent telemetry (routingKey={routingKey}): {jsonTelemetry}");

            // Wait 2 seconds
            await Task.Delay(2000);
        }
    }
}
