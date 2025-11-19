using System;
using F1Telemetry.Models;

namespace F1Telemetry.Services
{
    public class TelemetryDataGenerator
    {
        private readonly Random _random = new Random();
        private int _currentSector = 1;
        private int _currentLap = 1;
        private int _sectorDataCount = 0;
        private const int DataPointsPerSector = 5;

        public CarData GenerateTelemetryData(string carId = "#14")
        {
            var telemetry = new CarData
            {
                CarId = carId,
                Timestamp = DateTime.UtcNow,
                Sector = _currentSector,

                // Main telemetry
                SpeedKph = RandomDouble(150, 330),
                Throttle = RandomDouble(0, 100),
                Brake = RandomDouble(0, 100),
                EngineTempC = RandomDouble(90, 110),
                FuelLevel = Math.Max(0, 100 - (_currentLap - 1) * 5),

                // Tyres
                FrontLeftTyreTempC = RandomDouble(70, 90),
                FrontRightTyreTempC = RandomDouble(70, 90),
                RearLeftTyreTempC = RandomDouble(70, 90),
                RearRightTyreTempC = RandomDouble(70, 90),

                FrontLeftTyrePressure = RandomDouble(20, 22),
                FrontRightTyrePressure = RandomDouble(20, 22),
                RearLeftTyrePressure = RandomDouble(20, 22),
                RearRightTyrePressure = RandomDouble(20, 22)
            };

            _sectorDataCount++;
            if (_sectorDataCount >= DataPointsPerSector)
            {
                _sectorDataCount = 0;
                _currentSector++;
                if (_currentSector > 3)
                {
                    _currentSector = 1;
                    _currentLap++;
                }
            }

            return telemetry;
        }

        private double RandomDouble(double min, double max)
        {
            return min + _random.NextDouble() * (max - min);
        }
    }
}
