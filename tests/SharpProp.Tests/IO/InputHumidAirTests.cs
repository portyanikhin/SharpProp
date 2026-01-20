using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

public static class InputHumidAirTests
{
    [Theory]
    [MemberData(nameof(CoolPropKeys))]
    public static void CoolPropKey_AllInputs_MatchesWithCoolProp(
        InputHumidAir input,
        string coolPropKey
    ) => input.CoolPropKey.Should().Be(coolPropKey);

    [Theory]
    [MemberData(nameof(Values))]
    public static void Value_AllInputs_ShouldBeInSIUnits(InputHumidAir input, double value) =>
        input.Value.Should().Be(value);

    [Theory]
    [InlineData(-5000.1)]
    [InlineData(11000.1)]
    public static void Altitude_WrongValue_ThrowsArgumentOutOfRangeException(double altitude)
    {
        Action action = () => InputHumidAir.Altitude(altitude.Meters());
        action
            .Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Altitude above sea level should be between -5 000 and 11 000 meters!*");
    }

    [Fact]
    public static void Equals_Same_ReturnsTrue() =>
        InputHumidAir.Pressure(1.Atmospheres()).Should().Be(InputHumidAir.Altitude(0.Meters()));

    [Fact]
    public static void Equals_Other_ReturnsFalse() =>
        InputHumidAir
            .Pressure(1.Atmospheres())
            .Should()
            .NotBe(InputHumidAir.Temperature(20.DegreesCelsius()));

    public static IEnumerable<object[]> CoolPropKeys() =>
        [
            [InputHumidAir.Altitude(300.Meters()), "P"],
            [InputHumidAir.Density(1.25.KilogramsPerCubicMeter()), "Vha"],
            [InputHumidAir.DewTemperature(10.DegreesCelsius()), "D"],
            [InputHumidAir.Enthalpy(20.KilojoulesPerKilogram()), "Hha"],
            [InputHumidAir.Entropy(10.KilojoulesPerKilogramKelvin()), "Sha"],
            [InputHumidAir.Humidity(5.PartsPerThousand()), "W"],
            [InputHumidAir.PartialPressure(1.Kilopascals()), "P_w"],
            [InputHumidAir.Pressure(1.Atmospheres()), "P"],
            [InputHumidAir.RelativeHumidity(RelativeHumidity.FromPercent(50)), "R"],
            [InputHumidAir.SpecificVolume(0.8.CubicMetersPerKilogram()), "Vha"],
            [InputHumidAir.Temperature(20.DegreesCelsius()), "T"],
            [InputHumidAir.WetBulbTemperature(15.DegreesCelsius()), "B"],
        ];

    public static IEnumerable<object[]> Values() =>
        [
            [InputHumidAir.Altitude(300.Meters()), 97772.56060611102],
            [InputHumidAir.Density(1.25.KilogramsPerCubicMeter()), 0.8],
            [InputHumidAir.DewTemperature(10.DegreesCelsius()), 283.15],
            [InputHumidAir.Enthalpy(20.KilojoulesPerKilogram()), 2e4],
            [InputHumidAir.Entropy(10.KilojoulesPerKilogramKelvin()), 1e4],
            [InputHumidAir.Humidity(5.PartsPerThousand()), 5e-3],
            [InputHumidAir.PartialPressure(1.Kilopascals()), 1e3],
            [InputHumidAir.Pressure(1.Atmospheres()), 101325],
            [InputHumidAir.RelativeHumidity(RelativeHumidity.FromPercent(50)), 0.5],
            [InputHumidAir.SpecificVolume(0.8.CubicMetersPerKilogram()), 0.8],
            [InputHumidAir.Temperature(20.DegreesCelsius()), 293.15],
            [InputHumidAir.WetBulbTemperature(15.DegreesCelsius()), 288.15],
        ];
}
