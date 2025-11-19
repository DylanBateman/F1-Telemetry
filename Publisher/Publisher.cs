using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using F1Telemetry.Services;
class Publisher
{
    static async Task Main(string[] args)
    {
        var generator = new TelemetryDataGenerator();

        while (true)
        {
            var telemetry = generator.GenerateTelemetryData();

            Console.WriteLine($"telemetry: Speed: {telemetry.SpeedKph}, Front Left: {telemetry.FrontLeftTyreTempC}, Front Right: {telemetry.FrontRightTyreTempC}, Rear Left: {telemetry.RearLeftTyreTempC}, Rear Right: {telemetry.RearRightTyreTempC}");

            // Wait 2 seconds
            await Task.Delay(2000);
        }
    }
}
