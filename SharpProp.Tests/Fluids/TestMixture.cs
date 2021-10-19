using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using SharpProp.Outputs;

namespace SharpProp.Tests
{
    public class TestMixture
    {
        private static readonly List<FluidsList> Fluids = new() {FluidsList.Water, FluidsList.Ethanol};
        private static readonly List<double> Fractions = new() {0.5, 0.5};

        private static object[] _mixtureCases =
        {
            new object[]
            {
                new List<FluidsList> {FluidsList.Water}, new List<double> {0.5, 0.5},
                "Invalid input! Fluids and Fractions should be of the same length."
            },
            new object[]
            {
                new List<FluidsList> {FluidsList.MPG, FluidsList.MEG}, Fractions,
                "Invalid components! All of them should be a pure fluid with HEOS backend."
            },
            new object[]
            {
                Fluids, new List<double> {-2, 0.5},
                "Invalid component mass fractions! All of them should be in (0;1)."
            },
            new object[]
            {
                Fluids, new List<double> {0.5, 2},
                "Invalid component mass fractions! All of them should be in (0;1)."
            },
            new object[]
            {
                Fluids, new List<double> {0.8, 0.8},
                "Invalid component mass fractions! Their sum should be equal to 1."
            }
        };

        private readonly Mixture _mixture = new(Fluids, Fractions);

        [TestCaseSource(nameof(_mixtureCases))]
        public static void TestInitThrows(List<FluidsList> fluids, List<double> fractions, string message)
        {
            Action action = () => _ = new Mixture(fluids, fractions);
            action.Should().Throw<ArgumentException>().WithMessage(message);
        }

        [Test]
        public void TestEquals()
        {
            var mixtureWithState = _mixture.WithState(Input.Pressure(101325), Input.Temperature(293.15));
            var mixtureWithSameState = _mixture.WithState(Input.Pressure(101325), Input.Temperature(293.15));
            mixtureWithState.Should().Be(mixtureWithState);
            mixtureWithState.Should().BeSameAs(mixtureWithState);
            mixtureWithState.Should().NotBeNull();
            mixtureWithState.Equals(new object()).Should().BeFalse();
            mixtureWithState.Should().Be(mixtureWithSameState);
            mixtureWithState.Should().NotBeSameAs(mixtureWithSameState);
            (mixtureWithState == mixtureWithSameState).Should().Be(mixtureWithState.Equals(mixtureWithSameState));
            (mixtureWithState != _mixture).Should().Be(!mixtureWithState.Equals(_mixture));
        }

        [Test]
        public void TestFactory()
        {
            var clonedMixture = _mixture.Factory();
            clonedMixture.Should().Be(_mixture);
            clonedMixture.Should().NotBeSameAs(_mixture);
            clonedMixture.GetHashCode().Should().Be(_mixture.GetHashCode());
            clonedMixture.Fluids.Should().BeSameAs(_mixture.Fluids);
            clonedMixture.Fractions.Should().BeSameAs(_mixture.Fractions);
            clonedMixture.Phase.Should().Be(Phases.Unknown);
        }

        [Test]
        public void TestWithState()
        {
            var mixtureWithState = _mixture.WithState(Input.Pressure(101325), Input.Temperature(293.15));
            mixtureWithState.Should().NotBe(_mixture);
            mixtureWithState.GetHashCode().Should().NotBe(_mixture.GetHashCode());
            mixtureWithState.Phase.Should().Be(Phases.Liquid);
        }

        [Test]
        public void TestAsJson()
        {
            Jsonable mixture = _mixture.WithState(Input.Pressure(101325), Input.Temperature(293.15));
            mixture.AsJson().Should().Be(JsonConvert.SerializeObject(mixture,
                new JsonSerializerSettings {Converters = new List<JsonConverter> {new StringEnumConverter()}}));
        }
    }
}