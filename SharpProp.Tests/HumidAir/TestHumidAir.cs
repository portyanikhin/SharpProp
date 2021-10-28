using System;
using System.Collections.Generic;
using CoolProp;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using SharpProp.Extensions;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Serialization.JsonNet;

namespace SharpProp.Tests
{
    public class TestHumidAir
    {
        private readonly HumidAir _humidAir = new();

        [Test]
        public void TestEquals()
        {
            var humidAirWithState = HumidAir.WithState(InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()), InputHumidAir.RelativeHumidity(50.Percent()));
            var humidAirWithSameState = HumidAir.WithState(InputHumidAir.Pressure(101325.Pascals()),
                InputHumidAir.Temperature(293.15.Kelvins()), InputHumidAir.RelativeHumidity(50.Percent()));
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
            var humidAirWithState = HumidAir.WithState(InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()), InputHumidAir.RelativeHumidity(50.Percent()));
            humidAirWithState.GetHashCode().Should().NotBe(_humidAir.GetHashCode());
        }

        [Test]
        public void TestInvalidInput()
        {
            Action action = () => _humidAir.Update(InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()), InputHumidAir.Temperature(30.DegreesCelsius()));
            action.Should().Throw<ArgumentException>().WithMessage("Need to define 3 unique inputs!");
        }

        [Test]
        public void TestInvalidState()
        {
            Action action = () =>
            {
                _humidAir.Update(InputHumidAir.Pressure(1.Atmospheres()),
                    InputHumidAir.Temperature(20.DegreesCelsius()), InputHumidAir.RelativeHumidity(200.Percent()));
                var _ = _humidAir.Density;
            };
            action.Should().Throw<ArgumentException>().WithMessage("Invalid or not defined state!");
        }

        [Test]
        [Combinatorial]
        public void TestUpdate([Values(0.5e5, 1e5, 2e5, 5e5)] double pressure,
            [Range(253.15, 323.15, 10)] double temperature,
            [Range(0, 1, 0.1)] double relativeHumidity)
        {
            _humidAir.Update(InputHumidAir.Pressure(pressure.Pascals()), 
                InputHumidAir.Temperature(temperature.Kelvins()),
                InputHumidAir.RelativeHumidity((relativeHumidity * 1e2).Percent()));

            var actual = new List<double>
            {
                _humidAir.Compressibility, 
                _humidAir.Conductivity.WattsPerMeterKelvin, 
                _humidAir.Density.KilogramsPerCubicMeter, 
                _humidAir.DewTemperature.Kelvins,
                _humidAir.DynamicViscosity.PascalSeconds, 
                _humidAir.Enthalpy.JoulesPerKilogram, 
                _humidAir.Entropy.JoulesPerKilogramKelvin, 
                _humidAir.Humidity.DecimalFractions,
                _humidAir.PartialPressure.Pascals, 
                _humidAir.Pressure.Pascals, 
                Ratio.FromPercent(_humidAir.RelativeHumidity.Percent).DecimalFractions, 
                _humidAir.SpecificHeat.JoulesPerKilogramKelvin,
                _humidAir.Temperature.Kelvins, 
                _humidAir.WetBulbTemperature.Kelvins
            };
            var keys = new List<string>
            {
                "Z", "K", "Vha", "D", "M", "Hha", "Sha", "W", "P_w", "P", "R", "Cha", "T", "B"
            };
            for (var i = 0; i < keys.Count; i++)
            {
                var expected = HighLevelInterface(keys[i], pressure, temperature, relativeHumidity);
                expected = keys[i] == "Vha" ? 1 / expected : expected;
                actual[i].Should().BeApproximately(expected, 1e-6);
            }
        }

        [Test]
        public void TestAsJson()
        {
            var humidAir = HumidAir.WithState(InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()), InputHumidAir.RelativeHumidity(50.Percent()));
            humidAir.AsJson().Should().Be(
                JsonConvert.SerializeObject(humidAir, new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>
                        {new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()},
                    Formatting = Formatting.Indented
                }));
        }
        
        [Test]
        public void TestClone()
        {
            var humidAir = HumidAir.WithState(InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()), InputHumidAir.RelativeHumidity(50.Percent()));
            var clone = humidAir.Clone();
            clone.Should().Be(humidAir);
        }

        private static double HighLevelInterface(string key, double pressure, double temperature,
            double relativeHumidity) => CP.HAPropsSI(key, "P", pressure, "T", temperature, "R", relativeHumidity);
    }
}