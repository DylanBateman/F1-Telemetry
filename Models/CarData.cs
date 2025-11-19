using System;

namespace F1Telemetry.Models
{
    public class CarData
    {
        public string CarId { get; set; } = "#14";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public double SpeedKph { get; set; }

        public double FrontLeftTyreTempC { get; set; }
        public double FrontRightTyreTempC { get; set; }
        public double RearLeftTyreTempC { get; set; }
        public double RearRightTyreTempC { get; set; }

        public CarData() { }
        public CarData(string carId) => CarId = carId;
    }
}
