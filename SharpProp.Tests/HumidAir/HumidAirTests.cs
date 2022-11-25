using System;
using System.Collections.Generic;
using System.Linq;
using CoolProp;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToLength;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Serialization.JsonNet;
using Xunit;

namespace SharpProp.Tests
{
    [Collection("Fluids")]
    public class HumidAirTests
    {
        public HumidAirTests() => HumidAir = new HumidAir();

        private HumidAir HumidAir { get; }

        [Fact]
        public void Factory_Always_ReturnsNewInstanceWithNoDefinedState() =>
            HumidAir.Factory().Should().Be(new HumidAir());

        [Fact]
        public void WithState_InvalidState_ThrowsArgumentException()
        {
            Action action = () =>
                _ = HumidAir.WithState(
                        InputHumidAir.Pressure(1.Atmospheres()),
                        InputHumidAir.Temperature(20.DegreesCelsius()),
                        InputHumidAir.RelativeHumidity(200.Percent()))
                    .Density;
            action.Should().Throw<ArgumentException>()
                .WithMessage("Invalid or not defined state!");
        }

        [Fact]
        public void WithState_Always_ReturnsNewInstanceWithDefinedState() =>
            HumidAir.WithState(
                    InputHumidAir.Pressure(1.Atmospheres()),
                    InputHumidAir.Temperature(20.DegreesCelsius()),
                    InputHumidAir.RelativeHumidity(50.Percent()))
                .Should().NotBe(new HumidAir());

        [Fact]
        public void Update_SameInputs_ThrowsArgumentException()
        {
            Action action = () =>
                HumidAir.Update(
                    InputHumidAir.Pressure(1.Atmospheres()),
                    InputHumidAir.Temperature(20.DegreesCelsius()),
                    InputHumidAir.Temperature(30.DegreesCelsius()));
            action.Should().Throw<ArgumentException>()
                .WithMessage("Need to define 3 unique inputs!");
        }

        [Fact]
        public void Update_Always_InputsAreCached()
        {
            HumidAir.Update(
                InputHumidAir.Pressure(101325.Pascals()),
                InputHumidAir.Temperature(293.15.Kelvins()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            HumidAir.Pressure.Pascals.Should().Be(101325);
            HumidAir.Temperature.Kelvins.Should().Be(293.15);
            HumidAir.RelativeHumidity.Percent.Should().Be(50);
        }

        [Theory]
        [CombinatorialData]
        public void Update_VariousConditions_MatchesWithCoolProp(
            [CombinatorialValues(0.5, 1, 2, 5)] double pressure,
            [CombinatorialRange(-20, 50, 10)] double temperature,
            [CombinatorialRange(0, 100, 10)] double relativeHumidity)
        {
            HumidAir.Update(
                InputHumidAir.Pressure(pressure.Bars()),
                InputHumidAir.Temperature(temperature.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(relativeHumidity.Percent()));
            var actual = new List<double>
            {
                HumidAir.Compressibility,
                HumidAir.Conductivity.WattsPerMeterKelvin,
                HumidAir.Density.KilogramsPerCubicMeter,
                HumidAir.DewTemperature.Kelvins,
                HumidAir.DynamicViscosity.PascalSeconds,
                HumidAir.Enthalpy.JoulesPerKilogram,
                HumidAir.Entropy.JoulesPerKilogramKelvin,
                HumidAir.Humidity.DecimalFractions,
                HumidAir.PartialPressure.Pascals,
                HumidAir.Pressure.Pascals,
                Ratio.FromPercent(HumidAir.RelativeHumidity.Percent).DecimalFractions,
                HumidAir.SpecificHeat.JoulesPerKilogramKelvin,
                HumidAir.Temperature.Kelvins,
                HumidAir.WetBulbTemperature.Kelvins
            };
            var expected = new List<string>
            {
                "Z", "K", "Vha", "D", "M", "Hha", "Sha", "W", "P_w", "P", "R", "Cha", "T", "B"
            }.Select(CoolPropInterface).ToList();
            for (var i = 0; i < actual.Count; i++)
                actual[i].Should().BeApproximately(expected[i], 1e-9);
            HumidAir.KinematicViscosity.Should().Be(
                HumidAir.DynamicViscosity / HumidAir.Density);
            HumidAir.Prandtl.Should().Be(
                HumidAir.DynamicViscosity.PascalSeconds *
                HumidAir.SpecificHeat.JoulesPerKilogramKelvin /
                HumidAir.Conductivity.WattsPerMeterKelvin);
        }

        [Fact]
        public void Equals_Same_ReturnsTrue()
        {
            var origin = HumidAir.WithState(
                InputHumidAir.Altitude(0.Meters()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var same = HumidAir.WithState(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(293.15.Kelvins()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            origin.Should().Be(origin);
            origin.Should().BeSameAs(origin);
            origin.Should().Be(same);
            origin.Should().NotBeSameAs(same);
            (origin == same).Should().Be(origin.Equals(same));
        }

        [Fact]
        public void Equals_Other_ReturnsFalse()
        {
            var origin = HumidAir.WithState(
                InputHumidAir.Altitude(0.Meters()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var other = HumidAir.WithState(
                InputHumidAir.Altitude(0.Meters()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(60.Percent()));
            origin.Should().NotBe(other);
            origin.Should().NotBeNull();
            origin.Equals(new object()).Should().BeFalse();
            (origin != other).Should().Be(!origin.Equals(other));
        }

        [Fact]
        public void GetHashCode_Same_ReturnsSameHashCode()
        {
            var origin = HumidAir.WithState(
                InputHumidAir.Altitude(0.Meters()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var same = HumidAir.WithState(
                InputHumidAir.Pressure(1.Atmospheres()),
                InputHumidAir.Temperature(293.15.Kelvins()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            origin.GetHashCode().Should().Be(same.GetHashCode());
        }

        [Fact]
        public void GetHashCode_Other_ReturnsOtherHashCode()
        {
            var origin = HumidAir.WithState(
                InputHumidAir.Altitude(0.Meters()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var other = HumidAir.WithState(
                InputHumidAir.Altitude(0.Meters()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(60.Percent()));
            origin.GetHashCode().Should().NotBe(other.GetHashCode());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AsJson_IndentedOrNot_ReturnsProperlyFormattedJson(bool indented)
        {
            var humidAir = HumidAir.WithState(
                InputHumidAir.Altitude(0.Meters()),
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

        [Fact]
        public void Clone_Always_ReturnsNewInstanceWithSameState()
        {
            var origin = HumidAir.WithState(
                InputHumidAir.Altitude(0.Meters()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(50.Percent()));
            var clone = origin.Clone();
            clone.Should().Be(origin);
            clone.Update(InputHumidAir.Altitude(0.Meters()),
                InputHumidAir.Temperature(20.DegreesCelsius()),
                InputHumidAir.RelativeHumidity(60.Percent()));
            clone.Should().NotBe(origin);
        }

        private double CoolPropInterface(string key)
        {
            var value = CP.HAPropsSI(key,
                "P", HumidAir.Pressure.Pascals, "T", HumidAir.Temperature.Kelvins,
                "R", Ratio.FromPercent(HumidAir.RelativeHumidity.Percent).DecimalFractions);
            return key == "Vha" ? 1.0 / value : value;
        }
    }
}