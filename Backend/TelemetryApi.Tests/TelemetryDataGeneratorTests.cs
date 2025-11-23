using F1Telemetry.Services;
using FluentAssertions;
using Xunit;

namespace TelemetryApi.Tests;

public class TelemetryDataGeneratorTests
{
    private readonly TelemetryDataGenerator _generator = new();

    [Fact]
    public void CyclesSectorsInOrder()
    {
        var sectors = Enumerable.Range(0, 15)
            .Select(_ => _generator.GenerateTelemetryData().Sector)
            .ToArray();

        sectors.Should().ContainInOrder(1, 1, 1, 1, 1);
        sectors.Skip(5).Take(5).Should().OnlyContain(s => s == 2);
        sectors.Skip(10).Take(5).Should().OnlyContain(s => s == 3);
        sectors.Should().OnlyContain(s => s >= 1 && s <= 3);
    }

    [Fact]
    public void ProducesValuesWithinExpectedRange()
    {
        var sample = _generator.GenerateTelemetryData();

        sample.SpeedKph.Should().BeInRange(150, 330);
        sample.Throttle.Should().BeInRange(0, 100);
        sample.Brake.Should().BeInRange(0, 100);
        sample.EngineTempC.Should().BeInRange(90, 110);
        sample.FrontLeftTyrePressure.Should().BeInRange(20, 22);
    }
}
