using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

public static class InputHumidAirTests
{
    [Theory]
    [MemberData(nameof(CoolPropKeys))]
    public static void CoolPropKey_AllInputs_MatchesWithCoolProp(InputHumidAir input, string coolPropKey) =>
        input.CoolPropKey.Should().Be(coolPropKey);

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
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Altitude above sea level should be between -5000 and 11000 meters!*");
    }

    [Fact]
    public static void Equals_Same_ReturnsTrue() =>
        InputHumidAir.Pressure(1.Atmospheres())
            .Should().Be(InputHumidAir.Altitude(0.Meters()));

    [Fact]
    public static void Equals_Other_ReturnsFalse() =>
        InputHumidAir.Pressure(1.Atmospheres())
            .Should().NotBe(InputHumidAir.Temperature(20.DegreesCelsius()));

    public static IEnumerable<object[]> CoolPropKeys() =>
        new[]
        {
            new object[] {InputHumidAir.Altitude(300.Meters()), "P"},
            new object[] {InputHumidAir.Density(1.2.KilogramsPerCubicMeter()), "Vha"},
            new object[] {InputHumidAir.DewTemperature(10.DegreesCelsius()), "D"},
            new object[] {InputHumidAir.Enthalpy(20.KilojoulesPerKilogram()), "Hha"},
            new object[] {InputHumidAir.Entropy(10.KilojoulesPerKilogramKelvin()), "Sha"},
            new object[] {InputHumidAir.Humidity(5.PartsPerThousand()), "W"},
            new object[] {InputHumidAir.PartialPressure(1.Kilopascals()), "P_w"},
            new object[] {InputHumidAir.Pressure(1.Atmospheres()), "P"},
            new object[] {InputHumidAir.RelativeHumidity(RelativeHumidity.FromPercent(50)), "R"},
            new object[] {InputHumidAir.Temperature(20.DegreesCelsius()), "T"},
            new object[] {InputHumidAir.WetBulbTemperature(15.DegreesCelsius()), "B"}
        };

    public static IEnumerable<object[]> Values() =>
        new[]
        {
            new object[] {InputHumidAir.Altitude(300.Meters()), 97772.56060611102},
            new object[] {InputHumidAir.Density(1.2.KilogramsPerCubicMeter()), 0.8333333333333334},
            new object[] {InputHumidAir.DewTemperature(10.DegreesCelsius()), 283.15},
            new object[] {InputHumidAir.Enthalpy(20.KilojoulesPerKilogram()), 2e4},
            new object[] {InputHumidAir.Entropy(10.KilojoulesPerKilogramKelvin()), 1e4},
            new object[] {InputHumidAir.Humidity(5.PartsPerThousand()), 5e-3},
            new object[] {InputHumidAir.PartialPressure(1.Kilopascals()), 1e3},
            new object[] {InputHumidAir.Pressure(1.Atmospheres()), 101325},
            new object[] {InputHumidAir.RelativeHumidity(RelativeHumidity.FromPercent(50)), 0.5},
            new object[] {InputHumidAir.Temperature(20.DegreesCelsius()), 293.15},
            new object[] {InputHumidAir.WetBulbTemperature(15.DegreesCelsius()), 288.15}
        };
}