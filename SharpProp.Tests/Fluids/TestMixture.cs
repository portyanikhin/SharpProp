using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Serialization.JsonNet;

namespace SharpProp.Tests;

public static class TestMixture
{
    private static readonly List<FluidsList> Fluids =
        new() {FluidsList.Water, FluidsList.Ethanol};

    private static readonly List<Ratio> Fractions =
        new() {50.Percent(), 50.Percent()};

    private static readonly Mixture Mixture = new(Fluids, Fractions);

    private static readonly object[] MixtureCases =
    {
        new object[]
        {
            new List<FluidsList> {FluidsList.Water}, new List<Ratio> {50.Percent(), 50.Percent()},
            "Invalid input! Fluids and Fractions should be of the same length."
        },
        new object[]
        {
            new List<FluidsList> {FluidsList.MPG, FluidsList.MEG}, Fractions,
            "Invalid components! All of them should be a pure fluid with HEOS backend."
        },
        new object[]
        {
            Fluids, new List<Ratio> {-200.Percent(), 50.Percent()},
            "Invalid component mass fractions! All of them should be in (0;100) %."
        },
        new object[]
        {
            Fluids, new List<Ratio> {50.Percent(), 200.Percent()},
            "Invalid component mass fractions! All of them should be in (0;100) %."
        },
        new object[]
        {
            Fluids, new List<Ratio> {80.Percent(), 80.Percent()},
            "Invalid component mass fractions! Their sum should be equal to 100 %."
        }
    };

    [TestCaseSource(nameof(MixtureCases))]
    public static void TestInvalidInputs(List<FluidsList> fluids,
        List<Ratio> fractions, string message)
    {
        Action action = () => _ = new Mixture(fluids, fractions);
        action.Should().Throw<ArgumentException>().WithMessage(message);
    }

    [Test]
    public static void TestFactory()
    {
        var newMixture = Mixture.Factory();
        newMixture.Fluids.Should().BeEquivalentTo(Mixture.Fluids);
        newMixture.Fractions.Should().BeEquivalentTo(Mixture.Fractions);
        newMixture.Phase.Should().Be(Phases.Unknown);
    }

    [Test]
    public static void TestWithState()
    {
        var mixture = Mixture.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius()));
        mixture.Phase.Should().Be(Phases.Liquid);
    }

    [Test]
    public static void TestCachedInputs()
    {
        var mixture = Mixture.WithState(Input.Pressure(101325.Pascals()),
            Input.Temperature(293.15.Kelvins()));
        mixture.Pressure.Pascals.Should().Be(101325);
        mixture.Temperature.Kelvins.Should().Be(293.15);
    }

    [Test]
    public static void TestEquals()
    {
        var origin = Mixture.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius()));
        var same = Mixture.WithState(Input.Pressure(101325.Pascals()),
            Input.Temperature(293.15.Kelvins()));
        var other = Mixture.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(30.DegreesCelsius()));
        origin.Should().Be(origin);
        origin.Should().BeSameAs(origin);
        origin.Should().NotBe(other);
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
        var origin = Mixture.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius()));
        var same = Mixture.WithState(Input.Pressure(101325.Pascals()),
            Input.Temperature(293.15.Kelvins()));
        var other = Mixture.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(30.DegreesCelsius()));
        origin.GetHashCode().Should().Be(same.GetHashCode());
        origin.GetHashCode().Should().NotBe(other.GetHashCode());
    }

    [TestCase(true)]
    [TestCase(false)]
    public static void TestAsJson(bool indented)
    {
        var mixture = Mixture.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius()));
        mixture.AsJson(indented).Should().Be(
            JsonConvert.SerializeObject(mixture, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                    {new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()},
                Formatting = indented ? Formatting.Indented : Formatting.None
            }));
    }

    [Test]
    public static void TestClone()
    {
        var origin = Mixture.WithState(Input.Pressure(1.Atmospheres()),
            Input.Temperature(20.DegreesCelsius()));
        var clone = origin.Clone();
        clone.Should().Be(origin);
        clone.Update(Input.Pressure(1.Atmospheres()),
            Input.Temperature(30.DegreesCelsius()));
        clone.Should().NotBe(origin);
    }

    [Test]
    public static void TestDispose() => Mixture.Factory().Dispose();
}