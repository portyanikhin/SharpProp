using System;
using System.Collections.Generic;
using CoolProp;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using SharpProp.Outputs;

namespace SharpProp.Tests
{
    public class TestHumidAir
    {
        private readonly HumidAir _humidAir = new();

        [Test]
        public void TestEquals()
        {
            var humidAirWithState = HumidAir.WithState(InputHumidAir.Pressure(101325),
                InputHumidAir.Temperature(293.15), InputHumidAir.RelativeHumidity(0.5));
            var humidAirWithSameState = HumidAir.WithState(InputHumidAir.Pressure(101325),
                InputHumidAir.Temperature(293.15), InputHumidAir.RelativeHumidity(0.5));
            humidAirWithState.Should().Be(humidAirWithState);
            humidAirWithState.Should().BeSameAs(humidAirWithState);
            humidAirWithState.Should().NotBeNull();
            humidAirWithState.Equals(new object()).Should().BeFalse();
            humidAirWithState.Should().Be(humidAirWithSameState);
            humidAirWithState.Should().NotBeSameAs(humidAirWithSameState);
            (humidAirWithState == humidAirWithSameState).Should().Be(humidAirWithState.Equals(humidAirWithSameState));
            (humidAirWithState != _humidAir).Should().Be(!humidAirWithState.Equals(_humidAir));
        }

        [Test]
        public void TestWithState()
        {
            var humidAirWithState = HumidAir.WithState(InputHumidAir.Pressure(101325),
                InputHumidAir.Temperature(293.15), InputHumidAir.RelativeHumidity(0.5));
            humidAirWithState.GetHashCode().Should().NotBe(_humidAir.GetHashCode());
        }

        [Test]
        public void TestInvalidInput()
        {
            Action act = () => _humidAir.Update(InputHumidAir.Pressure(101325),
                InputHumidAir.Temperature(293.15), InputHumidAir.Temperature(303.15));
            act.Should().Throw<ArgumentException>().WithMessage("Need to define 3 unique inputs!");
        }

        [Test]
        public void TestInvalidState()
        {
            Action act = () =>
            {
                _humidAir.Update(InputHumidAir.Pressure(101325),
                    InputHumidAir.Temperature(293.15), InputHumidAir.RelativeHumidity(2));
                var _ = _humidAir.Density;
            };
            act.Should().Throw<ArgumentException>().WithMessage("Invalid or not defined state!");
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
                actual[i].Should().Be(expected);
            }
        }

        [Test]
        public void TestAsJson()
        {
            Jsonable humidAir = HumidAir.WithState(InputHumidAir.Pressure(101325),
                InputHumidAir.Temperature(293.15), InputHumidAir.RelativeHumidity(0.5));
            humidAir.AsJson().Should().Be(JsonConvert.SerializeObject(humidAir,
                new JsonSerializerSettings {Converters = new List<JsonConverter> {new StringEnumConverter()}}));
        }

        private static double HighLevelInterface(string key, double pressure, double temperature,
            double relativeHumidity) => CP.HAPropsSI(key, "P", pressure, "T", temperature, "R", relativeHumidity);
    }
}