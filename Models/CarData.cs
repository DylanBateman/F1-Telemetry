using System;

namespace F1Telemetry.Models
{
    public class CarData
    {
        public string CarId { get; set; } = "#14";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int Sector { get; set; }
        public double SpeedKph { get; set; }
        public double Throttle { get; set; }
        public double Brake { get; set; }
        public double EngineTempC { get; set; }
        public double FuelLevel { get; set; }
        public double FrontLeftTyreTempC { get; set; }
        public double FrontRightTyreTempC { get; set; }
        public double RearLeftTyreTempC { get; set; }
        public double RearRightTyreTempC { get; set; }
        public double FrontLeftTyrePressure { get; set; }
        public double FrontRightTyrePressure { get; set; }
        public double RearLeftTyrePressure { get; set; }
        public double RearRightTyrePressure { get; set; }

        public CarData() { }
        public CarData(string carId)
        {
            CarId = carId;
        }
    }
}
