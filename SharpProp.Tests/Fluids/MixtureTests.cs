using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Serialization.JsonNet;
using Xunit;

namespace SharpProp.Tests
{
    [Collection("Fluids")]
    public class MixtureTests : IDisposable
    {
        public MixtureTests() =>
            Mixture = new Mixture(
                new List<FluidsList> {FluidsList.Water, FluidsList.Ethanol},
                new List<Ratio> {60.Percent(), 40.Percent()});

        private Mixture Mixture { get; }

        public void Dispose()
        {
            Mixture.Dispose();
            GC.SuppressFinalize(this);
        }

        [Theory]
        [MemberData(nameof(WrongFluidsOrFractions))]
        public static void Mixture_WrongFluidsOrFractions_ThrowsArgumentException(
            List<FluidsList> fluids, List<Ratio> fractions, string message)
        {
            Action action = () => _ = new Mixture(fluids, fractions);
            action.Should().Throw<ArgumentException>().WithMessage(message);
        }

        [Fact]
        public void Factory_Always_FluidsAreConstant() =>
            Mixture.Factory().Fluids.Should().BeEquivalentTo(Mixture.Fluids);

        [Fact]
        public void Factory_Always_FractionsAreConstant() =>
            Mixture.Factory().Fractions.Should().BeEquivalentTo(Mixture.Fractions);

        [Fact]
        public void Factory_Always_PhaseIsUnknown() =>
            Mixture.Factory().Phase.Should().Be(Phases.Unknown);

        [Fact]
        public void WithState_VodkaInStandardConditions_PhaseIsLiquid() =>
            Mixture.WithState(Input.Pressure(1.Atmospheres()),
                    Input.Temperature(20.DegreesCelsius()))
                .Phase.Should().Be(Phases.Liquid);

        [Fact]
        public void Update_SameInputs_ThrowsArgumentException()
        {
            Action action = () =>
                Mixture.Update(Input.Pressure(1.Atmospheres()),
                    Input.Pressure(101325.Pascals()));
            action.Should().Throw<ArgumentException>()
                .WithMessage("Need to define 2 unique inputs!");
        }

        [Fact]
        public void Update_Always_InputsAreCached()
        {
            Mixture.Update(Input.Pressure(101325.Pascals()),
                Input.Temperature(293.15.Kelvins()));
            Mixture.Pressure.Pascals.Should().Be(101325);
            Mixture.Temperature.Kelvins.Should().Be(293.15);
        }

        [Fact]
        public void Equals_Same_ReturnsTrue()
        {
            var origin = Mixture.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            var same = Mixture.WithState(Input.Pressure(101325.Pascals()),
                Input.Temperature(293.15.Kelvins()));
            origin.Should().Be(origin);
            origin.Should().BeSameAs(origin);
            origin.Should().Be(same);
            origin.Should().NotBeSameAs(same);
            (origin == same).Should().Be(origin.Equals(same));
        }

        [Fact]
        public void Equals_Other_ReturnsFalse()
        {
            var origin = Mixture.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            var other = Mixture.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(30.DegreesCelsius()));
            origin.Should().NotBe(other);
            origin.Should().NotBeNull();
            origin.Equals(new object()).Should().BeFalse();
            (origin != other).Should().Be(!origin.Equals(other));
        }

        [Fact]
        public void GetHashCode_Same_ReturnsSameHashCode()
        {
            var origin = Mixture.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            var same = Mixture.WithState(Input.Pressure(101325.Pascals()),
                Input.Temperature(293.15.Kelvins()));
            origin.GetHashCode().Should().Be(same.GetHashCode());
        }

        [Fact]
        public void GetHashCode_Other_ReturnsOtherHashCode()
        {
            var origin = Mixture.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            var other = Mixture.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(30.DegreesCelsius()));
            origin.GetHashCode().Should().NotBe(other.GetHashCode());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AsJson_IndentedOrNot_ReturnsProperlyFormattedJson(bool indented)
        {
            var fluid = Mixture.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            fluid.AsJson(indented).Should().Be(
                JsonConvert.SerializeObject(fluid, new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>
                        {new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()},
                    Formatting = indented ? Formatting.Indented : Formatting.None
                }));
        }

        [Fact]
        public void Clone_Always_ReturnsNewInstanceWithSameState()
        {
            var origin = Mixture.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            var clone = origin.Clone();
            clone.Should().Be(origin);
            clone.Update(Input.Pressure(1.Atmospheres()),
                Input.Temperature(30.DegreesCelsius()));
            clone.Should().NotBe(origin);
        }

        public static IEnumerable<object[]> WrongFluidsOrFractions() =>
            new[]
            {
                new object[]
                {
                    new List<FluidsList> {FluidsList.Water},
                    new List<Ratio> {60.Percent(), 40.Percent()},
                    "Invalid input! Fluids and Fractions should be of the same length."
                },
                new object[]
                {
                    new List<FluidsList> {FluidsList.MPG, FluidsList.MEG},
                    new List<Ratio> {60.Percent(), 40.Percent()},
                    "Invalid components! All of them should be a pure fluid with HEOS backend."
                },
                new object[]
                {
                    new List<FluidsList> {FluidsList.Water, FluidsList.Ethanol},
                    new List<Ratio> {-200.Percent(), 40.Percent()},
                    "Invalid component mass fractions! All of them should be in (0;100) %."
                },
                new object[]
                {
                    new List<FluidsList> {FluidsList.Water, FluidsList.Ethanol},
                    new List<Ratio> {60.Percent(), 200.Percent()},
                    "Invalid component mass fractions! All of them should be in (0;100) %."
                },
                new object[]
                {
                    new List<FluidsList> {FluidsList.Water, FluidsList.Ethanol},
                    new List<Ratio> {80.Percent(), 80.Percent()},
                    "Invalid component mass fractions! Their sum should be equal to 100 %."
                }
            };
    }
}