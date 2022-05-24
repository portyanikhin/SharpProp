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
        [Test]
        public void TestFactory() =>
            new HumidAir().Factory().Should().Be(new HumidAir());

        [Test]
        public void TestWithState() =>
            new HumidAir().WithState(
                    InputHumidAir.Pressure(1.Atmospheres()),
                    InputHumidAir.Temperature(20.DegreesCelsius()),
                    InputHumidAir.RelativeHumidity(50.Percent()))
                .GetHashCode().Should().NotBe(new HumidAir().GetHashCode());

        [Test]
        public void TestInvalidInput()
        {
            Action action =
                () => _ = new HumidAir().WithState(
                    InputHumidAir.Pressure(1.Atmospheres()),
                    InputHumidAir.Temperature(20.DegreesCelsius()),
                    InputHumidAir.Temperature(30.DegreesCelsius()));
            action.Should().Throw<ArgumentException>()
                .WithMessage("Need to define 3 unique inputs!");
        }

        [Test]
        public void TestInvalidState()
        {
            Action action = () => _ = new HumidAir().WithState(
                    InputHumidAir.Pressure(1.Atmospheres()),
                    InputHumidAir.Temperature(20.DegreesCelsius()),
                    InputHumidAir.RelativeHumidity(200.Percent()))
                .Density;
            action.Should().Throw<ArgumentException>()
                .WithMessage("Invalid or not defined state!");
        }

        [Test]
        [Combinatorial]
        public void TestUpdate([Values(0.5e5, 1e5, 2e5, 5e5)] double pressure,
            [Range(253.15, 323.15, 10)] double temperature,
            [Range(0, 1, 0.1)] double relativeHumidity)
        {
            var humidAir = new HumidAir().WithState(
                InputHumidAir.Pressure(pressure.Pascals()),
                InputHumidAir.Temperature(temperature.Kelvins()),
                InputHumidAir.RelativeHumidity((relativeHumidity * 1e2).Percent()));
            var actual = new List<double>
            {
                humidAir.Compressibility,
                humidAir.Conductivity.WattsPerMeterKelvin,
                humidAir.Density.KilogramsPerCubicMeter,
                humidAir.DewTemperature.Kelvins,
                humidAir.DynamicViscosity.PascalSeconds,
                humidAir.Enthalpy.JoulesPerKilogram,
                humidAir.Entropy.JoulesPerKilogramKelvin,
                humidAir.Humidity.DecimalFractions,
                humidAir.PartialPressure.Pascals,
                humidAir.Pressure.Pascals,
                Ratio.FromPercent(humidAir.RelativeHumidity.Percent).DecimalFractions,
                humidAir.SpecificHeat.JoulesPerKilogramKelvin,
                humidAir.Temperature.Kelvins,
                humidAir.WetBulbTemperature.Kelvins
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

            humidAir.KinematicViscosity.Should().Be(
                humidAir.DynamicViscosity / humidAir.Density);
            humidAir.Prandtl.Should().Be(
                humidAir.DynamicViscosity.PascalSeconds *
                humidAir.SpecificHeat.JoulesPerKilogramKelvin /
                humidAir.Conductivity.WattsPerMeterKelvin);
        }

        [Test]
        public void TestCachedInputs()
        {
            var humidAir = new HumidAir().WithState(
                InputHumidAir.Pressure(101325.Pascals()),
                InputHumidAir.Temperature(293.15.Kelvins()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            humidAir.Pressure.Pascals.Should().Be(101325);
            humidAir.Temperature.Kelvins.Should().Be(293.15);
            humidAir.RelativeHumidity.Percent.Should().Be(50);
        }

        [Test]
        public void TestEquals()
        {
            var humidAir = new HumidAir().WithState(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var sameHumidAir = new HumidAir().WithState(
                InputHumidAir.Pressure(101325.Pascals()),
                InputHumidAir.Temperature(293.15.Kelvins()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var otherHumidAir = new HumidAir().WithState(
                InputHumidAir.Pressure(101325.Pascals()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(60.Percent()));
            humidAir.Should().Be(humidAir);
            humidAir.Should().BeSameAs(humidAir);
            humidAir.Should().NotBeNull();
            humidAir.Equals(new object()).Should().BeFalse();
            humidAir.Should().Be(sameHumidAir);
            humidAir.Should().NotBeSameAs(sameHumidAir);
            (humidAir == sameHumidAir).Should().Be(humidAir.Equals(sameHumidAir));
            (humidAir != otherHumidAir).Should().Be(!humidAir.Equals(otherHumidAir));
        }

        [Test]
        public void TestAsJson()
        {
            var humidAir = new HumidAir().WithState(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
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
            var humidAir = new HumidAir().WithState(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var clone = humidAir.Clone();
            clone.Should().Be(humidAir);
            clone.Update(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(60.Percent()));
        }

        private static double HighLevelInterface(string key,
            double pressure, double temperature, double relativeHumidity) =>
            CP.HAPropsSI(key, "P", pressure, "T", temperature, "R", relativeHumidity);
    }
}