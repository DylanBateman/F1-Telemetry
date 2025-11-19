using System;
using F1Telemetry.Models;

namespace F1Telemetry.Services
{
    public class TelemetryDataGenerator
    {
        private readonly Random _random = new Random();

        public CarData GenerateTelemetryData()
        {
            return new CarData
            {
                Timestamp = DateTime.UtcNow,
                SpeedKph = RandomDouble(150, 330),
                FrontLeftTyreTempC = RandomDouble(95, 108),
                FrontRightTyreTempC = RandomDouble(95, 108),
                RearLeftTyreTempC = RandomDouble(95, 108),
                RearRightTyreTempC = RandomDouble(95, 108)
            };
        }

        private double RandomDouble(double min, double max)
        {
            return min + _random.NextDouble() * (max - min);
        }
    }
}
