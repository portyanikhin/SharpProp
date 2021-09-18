using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public static void TestInitThrows(List<FluidsList> fluids, List<double> fractions, string exceptionMessage)
        {
            var exception = Assert.Throws<ArgumentException>(() => new Mixture(fluids, fractions));
            Assert.AreEqual(exceptionMessage, exception?.Message);
        }

        [Test]
        public void TestFactory()
        {
            var clonedMixture = _mixture.Factory();
            Assert.AreNotEqual(_mixture.GetHashCode(), clonedMixture.GetHashCode());
            Assert.AreEqual(_mixture.Fluids, clonedMixture.Fluids);
            Assert.AreEqual(_mixture.Fractions, clonedMixture.Fractions);
            Assert.That(clonedMixture.Phase is Phases.Unknown);
        }

        [Test]
        public void TestWithState()
        {
            var mixtureWithState = _mixture.WithState(Input.Pressure(101325), Input.Temperature(293.15));
            Assert.AreNotEqual(_mixture.GetHashCode(), mixtureWithState.GetHashCode());
            Assert.That(mixtureWithState.Phase is Phases.Liquid);
        }

        [Test]
        public void TestAsJson()
        {
            Jsonable mixture = _mixture.WithState(Input.Pressure(101325), Input.Temperature(293.15));
            Assert.AreEqual(
                JsonConvert.SerializeObject(mixture,
                    new JsonSerializerSettings {Converters = new List<JsonConverter> {new StringEnumConverter()}}),
                mixture.AsJson());
        }
    }
}