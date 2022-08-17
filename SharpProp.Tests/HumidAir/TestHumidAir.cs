using System;
using System.Collections.Generic;
using CoolProp;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Serialization.JsonNet;

namespace SharpProp.Tests
{
    public static class TestHumidAir
    {
        [Test]
        public static void TestFactory() =>
            new HumidAir().Factory().Should().Be(new HumidAir());

        [Test]
        public static void TestWithState() =>
            new HumidAir().WithState(
                    InputHumidAir.Pressure(1.Atmospheres()),
                    InputHumidAir.Temperature(20.DegreesCelsius()),
                    InputHumidAir.RelativeHumidity(50.Percent()))
                .GetHashCode().Should().NotBe(new HumidAir().GetHashCode());

        [Test]
        public static void TestInvalidInput()
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
        public static void TestInvalidState()
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
        public static void TestUpdate([Values(0.5e5, 1e5, 2e5, 5e5)] double pressure,
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
        public static void TestCachedInputs()
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
        public static void TestEquals()
        {
            var origin = new HumidAir().WithState(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var same = new HumidAir().WithState(
                InputHumidAir.Pressure(101325.Pascals()),
                InputHumidAir.Temperature(293.15.Kelvins()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var other = new HumidAir().WithState(
                InputHumidAir.Pressure(101325.Pascals()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(60.Percent()));
            origin.Should().Be(origin);
            origin.Should().BeSameAs(origin);
            origin.Should().NotBeNull();
            origin.Equals(new object()).Should().BeFalse();
            origin.Should().Be(same);
            origin.Should().NotBeSameAs(same);
            (origin == same).Should().Be(origin.Equals(same));
            (origin != other).Should().Be(!origin.Equals(other));
        }

        [Test]
        public static void TestGetHashCode()
        {
            var origin = new HumidAir().WithState(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var same = new HumidAir().WithState(
                InputHumidAir.Pressure(101325.Pascals()),
                InputHumidAir.Temperature(293.15.Kelvins()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var other = new HumidAir().WithState(
                InputHumidAir.Pressure(101325.Pascals()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(60.Percent()));
            origin.GetHashCode().Should().Be(same.GetHashCode());
            origin.GetHashCode().Should().NotBe(other.GetHashCode());
        }

        [TestCase(true)]
        [TestCase(false)]
        public static void TestAsJson(bool indented)
        {
            var humidAir = new HumidAir().WithState(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            humidAir.AsJson(indented).Should().Be(
                JsonConvert.SerializeObject(humidAir, new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>
                        {new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()},
                    Formatting = indented ? Formatting.Indented : Formatting.None
                }));
        }

        [Test]
        public static void TestClone()
        {
            var origin = new HumidAir().WithState(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var clone = origin.Clone();
            clone.Should().Be(origin);
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