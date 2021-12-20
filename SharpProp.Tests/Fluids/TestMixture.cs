using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using SharpProp.Extensions;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Serialization.JsonNet;

namespace SharpProp.Tests;

public class TestMixture
{
    private static readonly List<FluidsList> Fluids = new() {FluidsList.Water, FluidsList.Ethanol};
    private static readonly List<Ratio> Fractions = new() {50.Percent(), 50.Percent()};

    private static object[] _mixtureCases =
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

    private readonly Mixture _mixture = new(Fluids, Fractions);

    [TestCaseSource(nameof(_mixtureCases))]
    public static void TestInitThrows(List<FluidsList> fluids, List<Ratio> fractions, string message)
    {
        Action action = () => _ = new Mixture(fluids, fractions);
        action.Should().Throw<ArgumentException>().WithMessage(message);
    }

    [Test]
    public void TestFactory()
    {
        var clonedMixture = _mixture.Factory();
        clonedMixture.Fluids.Should().BeSameAs(_mixture.Fluids);
        clonedMixture.Fractions.Should().BeEquivalentTo(_mixture.Fractions);
        clonedMixture.Phase.Should().Be(Phases.Unknown);
    }

    [Test]
    public void TestWithState()
    {
        var mixtureWithState =
            _mixture.WithState(Input.Pressure(1.Atmospheres()), Input.Temperature(20.DegreesCelsius()));
        mixtureWithState.Phase.Should().Be(Phases.Liquid);
    }

    [Test]
    public void TestCachedInputs()
    {
        var mixtureWithState =
            _mixture.WithState(Input.Pressure(101325.Pascals()), Input.Temperature(293.15.Kelvins()));
        mixtureWithState.Pressure.Pascals.Should().Be(101325);
        mixtureWithState.Temperature.Kelvins.Should().Be(293.15);
    }
        
    [Test]
    public void TestEquals()
    {
        var mixtureWithState =
            _mixture.WithState(Input.Pressure(1.Atmospheres()), Input.Temperature(20.DegreesCelsius()));
        var mixtureWithSameState =
            _mixture.WithState(Input.Pressure(101325.Pascals()), Input.Temperature(293.15.Kelvins()));
        var mixtureWithOtherState =
            _mixture.WithState(Input.Pressure(1.Atmospheres()), Input.Temperature(30.DegreesCelsius()));
        mixtureWithState.Should().Be(mixtureWithState);
        mixtureWithState.Should().BeSameAs(mixtureWithState);
        mixtureWithState.Should().NotBe(mixtureWithOtherState);
        mixtureWithState.Should().NotBeNull();
        mixtureWithState.Equals(new object()).Should().BeFalse();
        mixtureWithState.Should().Be(mixtureWithSameState);
        mixtureWithState.Should().NotBeSameAs(mixtureWithSameState);
        (mixtureWithState == mixtureWithSameState).Should().Be(mixtureWithState.Equals(mixtureWithSameState));
        (mixtureWithState != mixtureWithOtherState).Should().Be(!mixtureWithState.Equals(mixtureWithOtherState));
    }
        
    [Test]
    public void TestGetHashCode()
    {
        var mixtureWithState =
            _mixture.WithState(Input.Pressure(1.Atmospheres()), Input.Temperature(20.DegreesCelsius()));
        var mixtureWithSameState =
            _mixture.WithState(Input.Pressure(101325.Pascals()), Input.Temperature(293.15.Kelvins()));
        var mixtureWithOtherState =
            _mixture.WithState(Input.Pressure(1.Atmospheres()), Input.Temperature(30.DegreesCelsius()));
        mixtureWithState.GetHashCode().Should().Be(mixtureWithSameState.GetHashCode());
        mixtureWithState.GetHashCode().Should().NotBe(mixtureWithOtherState.GetHashCode());
    }
        
    [Test]
    public void TestAsJson()
    {
        var mixture = _mixture.WithState(Input.Pressure(1.Atmospheres()), Input.Temperature(20.DegreesCelsius()));
        mixture.AsJson().Should().Be(
            JsonConvert.SerializeObject(mixture, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                    {new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()},
                Formatting = Formatting.Indented
            }));
    }
        
    [Test]
    public void TestClone()
    {
        var mixture = _mixture.WithState(Input.Pressure(1.Atmospheres()), Input.Temperature(20.DegreesCelsius()));
        var clone = mixture.Clone();
        clone.Should().Be(mixture);
    }
}