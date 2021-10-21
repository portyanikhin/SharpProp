using CoolProp;
using FluentAssertions;
using NUnit.Framework;
using UnitsNet.NumberExtensions.NumberToDensity;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToSpecificEnergy;
using UnitsNet.NumberExtensions.NumberToSpecificEntropy;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace SharpProp.Tests
{
    public static class TestInput
    {
        private static object[] _inputCases =
        {
            new object[] {Input.Density(999.KilogramsPerCubicMeter()), Parameters.iDmass, 999},
            new object[] {Input.Enthalpy(1.KilojoulesPerKilogram()), Parameters.iHmass, 1e3},
            new object[] {Input.Entropy(5.KilojoulesPerKilogramKelvin()), Parameters.iSmass, 5e3},
            new object[] {Input.InternalEnergy(10.KilojoulesPerKilogram()), Parameters.iUmass, 1e4},
            new object[] {Input.Pressure(1.Atmospheres()), Parameters.iP, 101325},
            new object[] {Input.Quality(50.Percent()), Parameters.iQ, 0.5},
            new object[] {Input.Temperature(20.DegreesCelsius()), Parameters.iT, 293.15}
        };

        [TestCaseSource(nameof(_inputCases))]
        public static void TestCoolPropKey(Input input, Parameters coolPropKey, double value) =>
            input.CoolPropKey.Should().Be(coolPropKey);

        [TestCaseSource(nameof(_inputCases))]
        public static void TestValue(Input input, Parameters coolPropKey, double value) =>
            input.Value.Should().Be(value);

        [Test]
        public static void TestEquals() =>
            Input.Pressure(101325.Pascals()).Should().Be(Input.Pressure(1.Atmospheres()));
    }
}