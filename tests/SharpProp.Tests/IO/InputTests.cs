// cSpell:disable

using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

public static class InputTests
{
    [Theory]
    [MemberData(nameof(CoolPropKeys))]
    public static void CoolPropKey_AllInputs_MatchesWithCoolProp(
        Input input,
        parameters coolPropKey
    ) => input.CoolPropKey.Should().Be(coolPropKey);

    [Theory]
    [MemberData(nameof(Values))]
    public static void Value_AllInputs_ShouldBeInSIUnits(Input input, double value) =>
        input.Value.Should().Be(value);

    [Fact]
    public static void Equals_Same_ReturnsTrue() =>
        Input.Pressure(1.Atmospheres()).Should().Be(Input.Pressure(101325.Pascals()));

    [Fact]
    public static void Equals_Other_ReturnsFalse() =>
        Input.Pressure(1.Atmospheres()).Should().NotBe(Input.Temperature(20.DegreesCelsius()));

    public static TheoryData<Input, parameters> CoolPropKeys() =>
        new()
        {
            { Input.Density(999.KilogramsPerCubicMeter()), parameters.iDmass },
            { Input.Enthalpy(1.KilojoulesPerKilogram()), parameters.iHmass },
            { Input.Entropy(5.KilojoulesPerKilogramKelvin()), parameters.iSmass },
            { Input.InternalEnergy(10.KilojoulesPerKilogram()), parameters.iUmass },
            { Input.Pressure(1.Atmospheres()), parameters.iP },
            { Input.Quality(50.Percent()), parameters.iQ },
            { Input.SpecificVolume((1.0 / 999).CubicMetersPerKilogram()), parameters.iDmass },
            { Input.Temperature(20.DegreesCelsius()), parameters.iT },
        };

    public static TheoryData<Input, double> Values() =>
        new()
        {
            { Input.Density(999.KilogramsPerCubicMeter()), 999 },
            { Input.Enthalpy(1.KilojoulesPerKilogram()), 1e3 },
            { Input.Entropy(5.KilojoulesPerKilogramKelvin()), 5e3 },
            { Input.InternalEnergy(10.KilojoulesPerKilogram()), 1e4 },
            { Input.Pressure(1.Atmospheres()), 101325 },
            { Input.Quality(50.Percent()), 0.5 },
            { Input.SpecificVolume((1.0 / 999).CubicMetersPerKilogram()), 999 },
            { Input.Temperature(20.DegreesCelsius()), 293.15 },
        };
}
