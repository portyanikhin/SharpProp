using FluentAssertions;
using NUnit.Framework;

namespace SharpProp.Tests
{
    public static class TestInputHumidAir
    {
        private static object[] _inputHumidAirCases =
        {
            new object[] {InputHumidAir.Density(1.2), "Vha", 1.2},
            new object[] {InputHumidAir.DewTemperature(283.15), "D", 283.15},
            new object[] {InputHumidAir.Enthalpy(2e4), "Hha", 2e4},
            new object[] {InputHumidAir.Entropy(1e4), "Sha", 1e4},
            new object[] {InputHumidAir.Humidity(5e-3), "W", 5e-3},
            new object[] {InputHumidAir.PartialPressure(1e3), "P_w", 1e3},
            new object[] {InputHumidAir.Pressure(101325), "P", 101325},
            new object[] {InputHumidAir.RelativeHumidity(0.5), "R", 0.5},
            new object[] {InputHumidAir.Temperature(293.15), "T", 293.15},
            new object[] {InputHumidAir.WetBulbTemperature(288.15), "B", 288.15}
        };

        [TestCaseSource(nameof(_inputHumidAirCases))]
        public static void TestCoolPropKey(InputHumidAir input, string coolPropKey, double value) =>
            input.CoolPropKey.Should().Be(coolPropKey);

        [TestCaseSource(nameof(_inputHumidAirCases))]
        public static void TestValue(InputHumidAir input, string coolPropKey, double value) =>
            input.Value.Should().Be(value);
    }
}