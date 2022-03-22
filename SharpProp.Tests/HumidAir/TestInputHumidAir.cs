using FluentAssertions;
using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToDensity;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToSpecificEnergy;
using UnitsNet.NumberExtensions.NumberToSpecificEntropy;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace SharpProp.Tests
{
    public static class TestInputHumidAir
    {
        private static object[] _inputHumidAirCases =
        {
            new object[] {InputHumidAir.Density(1.2.KilogramsPerCubicMeter()), "Vha", 0.8333333333333334},
            new object[] {InputHumidAir.DewTemperature(10.DegreesCelsius()), "D", 283.15},
            new object[] {InputHumidAir.Enthalpy(20.KilojoulesPerKilogram()), "Hha", 2e4},
            new object[] {InputHumidAir.Entropy(10.KilojoulesPerKilogramKelvin()), "Sha", 1e4},
            new object[] {InputHumidAir.Humidity(5.PartsPerThousand()), "W", 5e-3},
            new object[] {InputHumidAir.PartialPressure(1.Kilopascals()), "P_w", 1e3},
            new object[] {InputHumidAir.Pressure(1.Atmospheres()), "P", 101325},
            new object[] {InputHumidAir.RelativeHumidity(RelativeHumidity.FromPercent(50)), "R", 0.5},
            new object[] {InputHumidAir.Temperature(20.DegreesCelsius()), "T", 293.15},
            new object[] {InputHumidAir.WetBulbTemperature(15.DegreesCelsius()), "B", 288.15}
        };

        [TestCaseSource(nameof(_inputHumidAirCases))]
        public static void TestCoolPropKey(InputHumidAir input, string coolPropKey, double value) =>
            input.CoolPropKey.Should().Be(coolPropKey);

        [TestCaseSource(nameof(_inputHumidAirCases))]
        public static void TestValue(InputHumidAir input, string coolPropKey, double value) =>
            input.Value.Should().Be(value);

        [Test]
        public static void TestEquals() =>
            InputHumidAir.Pressure(1.Atmospheres()).Should().Be(InputHumidAir.Pressure(101325.Pascals()));
    }
}