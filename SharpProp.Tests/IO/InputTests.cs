using System.Collections.Generic;
using FluentAssertions;
using UnitsNet.NumberExtensions.NumberToDensity;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToSpecificEnergy;
using UnitsNet.NumberExtensions.NumberToSpecificEntropy;
using UnitsNet.NumberExtensions.NumberToTemperature;
using Xunit;

namespace SharpProp.Tests;

public static class InputTests
{
    [Theory]
    [MemberData(nameof(CoolPropKeys))]
    public static void CoolPropKey_AllInputs_MatchesWithCoolProp(Input input, Parameters coolPropKey) =>
        input.CoolPropKey.Should().Be(coolPropKey);

    [Theory]
    [MemberData(nameof(Values))]
    public static void Value_AllInputs_ShouldBeInSIUnits(Input input, double value) =>
        input.Value.Should().Be(value);

    [Fact]
    public static void Equals_Same_ReturnsTrue() =>
        Input.Pressure(1.Atmospheres())
            .Should().Be(Input.Pressure(101325.Pascals()));

    [Fact]
    public static void Equals_Other_ReturnsFalse() =>
        Input.Pressure(1.Atmospheres())
            .Should().NotBe(Input.Temperature(20.DegreesCelsius()));

    public static IEnumerable<object[]> CoolPropKeys() =>
        new[]
        {
            new object[] {Input.Density(999.KilogramsPerCubicMeter()), Parameters.iDmass},
            new object[] {Input.Enthalpy(1.KilojoulesPerKilogram()), Parameters.iHmass},
            new object[] {Input.Entropy(5.KilojoulesPerKilogramKelvin()), Parameters.iSmass},
            new object[] {Input.InternalEnergy(10.KilojoulesPerKilogram()), Parameters.iUmass},
            new object[] {Input.Pressure(1.Atmospheres()), Parameters.iP},
            new object[] {Input.Quality(50.Percent()), Parameters.iQ},
            new object[] {Input.Temperature(20.DegreesCelsius()), Parameters.iT}
        };

    public static IEnumerable<object[]> Values() =>
        new[]
        {
            new object[] {Input.Density(999.KilogramsPerCubicMeter()), 999},
            new object[] {Input.Enthalpy(1.KilojoulesPerKilogram()), 1e3},
            new object[] {Input.Entropy(5.KilojoulesPerKilogramKelvin()), 5e3},
            new object[] {Input.InternalEnergy(10.KilojoulesPerKilogram()), 1e4},
            new object[] {Input.Pressure(1.Atmospheres()), 101325},
            new object[] {Input.Quality(50.Percent()), 0.5},
            new object[] {Input.Temperature(20.DegreesCelsius()), 293.15}
        };
}