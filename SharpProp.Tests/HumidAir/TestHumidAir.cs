using System;
using System.Collections.Generic;
using CoolProp;
using NUnit.Framework;

namespace SharpProp.Tests
{
    public class TestHumidAir
    {
        private readonly HumidAir _humidAir = new();

        [Test]
        public void TestWithState()
        {
            var humidAirWithState = HumidAir.WithState(InputHumidAir.Pressure(101325),
                InputHumidAir.Temperature(293.15), InputHumidAir.RelativeHumidity(0.5));
            Assert.AreNotEqual(_humidAir.GetHashCode(), humidAirWithState.GetHashCode());
        }

        [Test(ExpectedResult = "Need to define 3 unique inputs!")]
        public string? TestInvalidInput()
        {
            var exception = Assert.Throws<ArgumentException>(() => _humidAir.Update(InputHumidAir.Pressure(101325),
                InputHumidAir.Temperature(293.15), InputHumidAir.Temperature(303.15)));
            return exception?.Message;
        }

        [Test(ExpectedResult = "Invalid or not defined state!")]
        public string? TestInvalidState()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                _humidAir.Update(InputHumidAir.Pressure(101325),
                    InputHumidAir.Temperature(293.15), InputHumidAir.RelativeHumidity(2));
                var _ = _humidAir.Density;
            });
            return exception?.Message;
        }

        [Test]
        [Combinatorial]
        public void TestUpdate([Values(0.5e5, 1e5, 2e5, 5e5)] double pressure,
            [Range(253.15, 323.15, 10)] double temperature,
            [Range(0, 1, 0.1)] double relativeHumidity)
        {
            _humidAir.Update(InputHumidAir.Pressure(pressure), InputHumidAir.Temperature(temperature),
                InputHumidAir.RelativeHumidity(relativeHumidity));

            var actual = new List<double>
            {
                _humidAir.Compressibility, _humidAir.Conductivity, _humidAir.Density, _humidAir.DewTemperature,
                _humidAir.DynamicViscosity, _humidAir.Enthalpy, _humidAir.Entropy, _humidAir.Humidity,
                _humidAir.PartialPressure, _humidAir.Pressure, _humidAir.RelativeHumidity, _humidAir.SpecificHeat,
                _humidAir.Temperature, _humidAir.WetBulbTemperature
            };
            var keys = new List<string>
            {
                "Z", "K", "Vha", "D",
                "M", "Hha", "Sha", "W",
                "P_w", "P", "R", "Cha",
                "T", "B"
            };
            for (var i = 0; i < keys.Count; i++)
            {
                var expected = HighLevelInterface(keys[i], pressure, temperature, relativeHumidity);
                expected = keys[i] == "Vha" ? 1 / expected : expected;
                Assert.AreEqual(expected, actual[i]);
            }
        }

        private static double HighLevelInterface(string key, double pressure, double temperature,
            double relativeHumidity) => CP.HAPropsSI(key, "P", pressure, "T", temperature, "R", relativeHumidity);
    }
}