using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CoolProp;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using SharpProp.Extensions;
using SharpProp.Outputs;

namespace SharpProp.Tests
{
    public class TestFluid
    {
        private readonly Fluid _water = new(FluidsList.Water);
        private Fluid _fluid = null!;
        private double? _fraction;
        private double _pressure;
        private double _temperature;

        private void SetUp(FluidsList name, double pressure)
        {
            _fraction = SetUpFraction(name);
            _fluid = new Fluid(name, _fraction);
            _pressure = pressure;
            _temperature = SetUpTemperature(_fluid);
            _fluid.Update(Input.Pressure(_pressure), Input.Temperature(_temperature));
        }

        private static double? SetUpFraction(FluidsList name) =>
            name.Pure() ? null : 0.1 * name.FractionMin() + 0.9 * name.FractionMax();

        private static double SetUpTemperature(AbstractFluid fluid) =>
            0.1 * fluid.MinTemperature + 0.9 * fluid.MaxTemperature;

        [Test]
        public void TestFactory()
        {
            var clonedWater = _water.Factory();
            clonedWater.GetHashCode().Should().NotBe(_water.GetHashCode());
            clonedWater.Name.Should().Be(_water.Name);
            clonedWater.Fraction.Should().Be(_water.Fraction);
            clonedWater.Phase.Should().Be(Phases.Unknown);
        }

        [Test]
        public void TestWithState()
        {
            var waterWithState = _water.WithState(Input.Pressure(101325), Input.Temperature(293.15));
            waterWithState.GetHashCode().Should().NotBe(_water.GetHashCode());
            waterWithState.Phase.Should().Be(Phases.Liquid);
        }

        [TestCase(FluidsList.MPG, null, "Need to define fraction!")]
        [TestCase(FluidsList.MPG, -2, "Invalid fraction value! It should be in [0;0,6]. Entered value = -2")]
        [TestCase(FluidsList.MPG, 2, "Invalid fraction value! It should be in [0;0,6]. Entered value = 2")]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public static void TestInitThrows(FluidsList name, double? fraction, string message)
        {
            Action act = () => new Fluid(name, fraction);
            act.Should().Throw<ArgumentException>().WithMessage(message);
        }

        [Test]
        [Combinatorial]
        public void TestUpdate([Values] FluidsList name, [Values(1e7, 1e8)] double pressure)
        {
            if (name is FluidsList.AL or FluidsList.AN) Assert.Pass(); // Cause CoolProp error
            if (name.CoolPropName().EndsWith(".mix")) Assert.Pass();
            SetUp(name, pressure);
            var actual = new List<double?>
            {
                _fluid.Compressibility, _fluid.Conductivity, _fluid.CriticalPressure, _fluid.CriticalTemperature,
                _fluid.Density, _fluid.DynamicViscosity, _fluid.Enthalpy, _fluid.Entropy, _fluid.FreezingTemperature,
                _fluid.InternalEnergy, _fluid.MaxPressure, _fluid.MaxTemperature, _fluid.MinPressure,
                _fluid.MinTemperature, _fluid.MolarMass, _fluid.Prandtl, _fluid.Pressure, _fluid.Quality,
                _fluid.SoundSpeed, _fluid.SpecificHeat, _fluid.SurfaceTension, _fluid.Temperature,
                _fluid.TriplePressure, _fluid.TripleTemperature
            };
            var keys = new List<string>
            {
                "Z", "L", "p_critical", "T_critical",
                "D", "V", "H", "S", "T_freeze",
                "U", "P_max", "T_max", "P_min",
                "T_min", "M", "Prandtl", "P", "Q",
                "A", "C", "I", "T",
                "p_triple", "T_triple"
            };
            for (var i = 0; i < keys.Count; i++)
                if (keys[i] is not ("P" or "T"))
                    actual[i].Should().Be(HighLevelInterface(keys[i]));
        }

        [Test]
        public void TestUpdateThrows()
        {
            Action act = () => _water.Update(Input.Pressure(101325), Input.Pressure(101325));
            act.Should().Throw<ArgumentException>().WithMessage("Need to define 2 unique inputs!");
        }

        [Test]
        public void TestAsJson()
        {
            Jsonable water = _water.WithState(Input.Pressure(101325), Input.Temperature(293.15));
            water.AsJson().Should().Be(JsonConvert.SerializeObject(water,
                new JsonSerializerSettings {Converters = new List<JsonConverter> {new StringEnumConverter()}}));
        }

        private double? HighLevelInterface(string outputKey)
        {
            try
            {
                double value;
                if (_fraction.HasValue)
                {
                    value = CP.PropsSImulti(new StringVector {outputKey}, "P", new DoubleVector {_pressure}, "T",
                        new DoubleVector {_temperature}, _fluid.Name.CoolPropBackend(),
                        new StringVector {_fluid.Name.CoolPropName()},
                        new DoubleVector {_fraction.Value})[0][0];
                    return CheckedValue(value, outputKey);
                }

                value = CP.PropsSI(outputKey, "P", _pressure, "T", _temperature, CoolPropHighLevelName(_fluid.Name));
                return CheckedValue(value, outputKey);
            }
            catch (ApplicationException)
            {
                return null;
            }
        }

        private static string CoolPropHighLevelName(FluidsList name) =>
            name.CoolPropBackend() == "HEOS"
                ? name.CoolPropName()
                : $"{name.CoolPropBackend()}::{name.CoolPropName()}";

        private static double? CheckedValue(double value, string outputKey)
        {
            if (double.IsInfinity(value) || double.IsNaN(value) || outputKey == "Q" && value is < 0 or > 1)
                return null;
            return value;
        }
    }
}