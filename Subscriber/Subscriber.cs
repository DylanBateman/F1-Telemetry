using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

string bindingKey;

if (args.Length > 0)
{
    bindingKey = $"sector.{args[0]}";
    Console.WriteLine($"Subscribing to sector {args[0]}");
}
else
{
    bindingKey = "sector.*";
    Console.WriteLine("No sector specified. Subscribing to all sectors.");
}

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: "car-data", type: ExchangeType.Topic);

string queueName = $"car-data-subscriber-{Guid.NewGuid()}";
await channel.QueueDeclareAsync(
    queue: queueName,
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

await channel.QueueBindAsync(queue: queueName, exchange: "car-data", routingKey: bindingKey);

Console.WriteLine($" [*] Waiting for car data, for routing key: {bindingKey}");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    byte[] body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] {message}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();