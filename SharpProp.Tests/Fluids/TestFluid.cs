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
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Serialization.JsonNet;

namespace SharpProp.Tests
{
    public class TestFluid
    {
        private readonly Fluid _water = new(FluidsList.Water);
        private Fluid _fluid = null!;
        private Ratio? _fraction;
        private Pressure _pressure;
        private Temperature _temperature;

        private void SetUp(FluidsList name, Pressure pressure)
        {
            _fraction = SetUpFraction(name);
            _fluid = new Fluid(name, _fraction);
            _pressure = pressure;
            _temperature = SetUpTemperature(_fluid);
            _fluid.Update(Input.Pressure(_pressure), Input.Temperature(_temperature));
        }

        private static Ratio? SetUpFraction(FluidsList name) =>
            name.Pure()
                ? null
                : 0.1 * name.FractionMin() +
                  0.9 * name.FractionMax();

        private static Temperature SetUpTemperature(AbstractFluid fluid) =>
            (0.1 * fluid.MinTemperature.Kelvins +
             0.9 * fluid.MaxTemperature.Kelvins).Kelvins();

        [Test]
        public void TestFactory()
        {
            var clonedWater = _water.Factory();
            clonedWater.Name.Should().Be(_water.Name);
            clonedWater.Fraction.Should().Be(_water.Fraction);
            clonedWater.Phase.Should().Be(Phases.Unknown);
        }

        [Test]
        public void TestWithState()
        {
            var waterWithState = _water.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            waterWithState.Phase.Should().Be(Phases.Liquid);
        }

        [TestCase(FluidsList.MPG, null, "Need to define fraction!")]
        [TestCase(FluidsList.MPG, -2, "Invalid fraction value! It should be in [0;60] %. Entered value = -200 %.")]
        [TestCase(FluidsList.MPG, 2, "Invalid fraction value! It should be in [0;60] %. Entered value = 200 %.")]
        public static void TestInvalidFraction(FluidsList name, double? fraction, string message)
        {
            Action action = () =>
                _ = new Fluid(name, fraction.HasValue
                    ? Ratio.FromDecimalFractions(fraction.Value)
                    : null);
            action.Should().Throw<ArgumentException>().WithMessage(message);
        }

        [Test]
        [Combinatorial]
        public void TestUpdate([Values] FluidsList name, [Values(1e7, 1e8)] double pressure)
        {
            if (name is FluidsList.AL or FluidsList.AN) Assert.Pass(); // Cause CoolProp error
            if (name.CoolPropName().EndsWith(".mix")) Assert.Pass();
            SetUp(name, pressure.Pascals());
            var actual = new List<double?>
            {
                _fluid.Compressibility,
                _fluid.Conductivity?.WattsPerMeterKelvin,
                _fluid.CriticalPressure?.Pascals,
                _fluid.CriticalTemperature?.Kelvins,
                _fluid.Density.KilogramsPerCubicMeter,
                _fluid.DynamicViscosity?.PascalSeconds,
                _fluid.Enthalpy.JoulesPerKilogram,
                _fluid.Entropy.JoulesPerKilogramKelvin,
                _fluid.FreezingTemperature?.Kelvins,
                _fluid.InternalEnergy.JoulesPerKilogram,
                _fluid.MaxPressure?.Pascals,
                _fluid.MaxTemperature.Kelvins,
                _fluid.MinPressure?.Pascals,
                _fluid.MinTemperature.Kelvins,
                _fluid.MolarMass?.KilogramsPerMole,
                _fluid.Prandtl,
                _fluid.Pressure.Pascals,
                _fluid.Quality?.DecimalFractions,
                _fluid.SoundSpeed?.MetersPerSecond,
                _fluid.SpecificHeat.JoulesPerKilogramKelvin,
                _fluid.SurfaceTension?.NewtonsPerMeter,
                _fluid.Temperature.Kelvins,
                _fluid.TriplePressure?.Pascals,
                _fluid.TripleTemperature?.Kelvins
            };
            var keys = new List<string>
            {
                "Z", "L", "p_critical", "T_critical", "D", "V", "H", "S", "T_freeze", "U", "P_max", "T_max",
                "P_min", "T_min", "M", "Prandtl", "P", "Q", "A", "C", "I", "T", "p_triple", "T_triple"
            };
            for (var i = 0; i < keys.Count; i++)
                if (keys[i] is not ("P" or "T"))
                    actual[i].Should().BeApproximately(HighLevelInterface(keys[i]), 1e-9);
            _fluid.KinematicViscosity.Should().Be(_fluid.DynamicViscosity / _fluid.Density);
        }

        [Test]
        public void TestUpdateInvalidInput()
        {
            Action action =
                () => _water.Update(Input.Pressure(1.Atmospheres()),
                    Input.Pressure(101325.Pascals()));
            action.Should().Throw<ArgumentException>()
                .WithMessage("Need to define 2 unique inputs!");
        }

        [Test]
        public void TestCachedInputs()
        {
            var water = _water.WithState(Input.Pressure(101325.Pascals()),
                Input.Temperature(293.15.Kelvins()));
            water.Pressure.Pascals.Should().Be(101325);
            water.Temperature.Kelvins.Should().Be(293.15);
        }

        [Test]
        public void TestEquals()
        {
            var water = _water.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            var sameWater = _water.WithState(Input.Pressure(101325.Pascals()),
                Input.Temperature(293.15.Kelvins()));
            var otherWater = _water.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(30.DegreesCelsius()));
            water.Should().Be(water);
            water.Should().BeSameAs(water);
            water.Should().NotBe(otherWater);
            water.Should().NotBeNull();
            water.Equals(new object()).Should().BeFalse();
            water.Should().Be(sameWater);
            water.Should().NotBeSameAs(sameWater);
            (water == sameWater).Should().Be(water.Equals(sameWater));
            (water != otherWater).Should().Be(!water.Equals(otherWater));
        }

        [Test]
        public void TestGetHashCode()
        {
            var water = _water.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            var sameWater = _water.WithState(Input.Pressure(101325.Pascals()),
                Input.Temperature(293.15.Kelvins()));
            var otherWater = _water.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(30.DegreesCelsius()));
            water.GetHashCode().Should().Be(sameWater.GetHashCode());
            water.GetHashCode().Should().NotBe(otherWater.GetHashCode());
        }

        [Test]
        public void TestAsJson()
        {
            var water = _water.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            water.AsJson().Should().Be(
                JsonConvert.SerializeObject(water, new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>
                        {new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()},
                    Formatting = Formatting.Indented
                }));
        }

        [Test]
        public void TestClone()
        {
            var water = _water.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            var clone = water.Clone();
            clone.Should().Be(water);
            clone.Update(Input.Pressure(1.Atmospheres()),
                Input.Temperature(30.DegreesCelsius()));
            clone.Should().NotBe(water);
        }

        private double? HighLevelInterface(string outputKey)
        {
            try
            {
                double value;
                if (_fraction.HasValue)
                {
                    value = CP.PropsSImulti(new StringVector {outputKey},
                        "P", new DoubleVector {_pressure.Pascals},
                        "T", new DoubleVector {_temperature.Kelvins},
                        _fluid.Name.CoolPropBackend(),
                        new StringVector {_fluid.Name.CoolPropName()},
                        new DoubleVector {_fraction.Value.DecimalFractions})[0][0];
                    return CheckedValue(value, outputKey);
                }

                value = CP.PropsSI(outputKey, "P", _pressure.Pascals, "T", _temperature.Kelvins,
                    CoolPropHighLevelName(_fluid.Name));
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

        private static double? CheckedValue(double value, string outputKey) =>
            double.IsInfinity(value) ||
            double.IsNaN(value) ||
            (outputKey == "Q" && value is < 0 or > 1)
                ? null
                : value;
    }
}