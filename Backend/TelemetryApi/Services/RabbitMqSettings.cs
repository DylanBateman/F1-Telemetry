namespace TelemetryApi.Services
{
    public class RabbitMqSettings
    {
        public string HostName { get; set; } = "localhost";
        public string ExchangeName { get; set; } = "car-data";
    }
}
