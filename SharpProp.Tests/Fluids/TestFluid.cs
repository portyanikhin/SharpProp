using System;
using System.Collections.Generic;
using CoolProp;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Serialization.JsonNet;

namespace SharpProp.Tests
{
    public class TestFluid
    {
        private static readonly Fluid Water = new(FluidsList.Water);
        private Fluid Fluid { get; set; } = null!;
        private Ratio? Fraction { get; set; }
        private Pressure Pressure { get; set; }
        private Temperature Temperature { get; set; }

        [Test]
        public static void TestFactory()
        {
            var newWater = Water.Factory();
            newWater.Name.Should().Be(Water.Name);
            newWater.Fraction.Should().Be(Water.Fraction);
            newWater.Phase.Should().Be(Phases.Unknown);
        }

        [Test]
        public static void TestWithState() =>
            Water.WithState(Input.Pressure(1.Atmospheres()),
                    Input.Temperature(20.DegreesCelsius())).Phase
                .Should().Be(Phases.Liquid);

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
                Fluid.Compressibility,
                Fluid.Conductivity?.WattsPerMeterKelvin,
                Fluid.CriticalPressure?.Pascals,
                Fluid.CriticalTemperature?.Kelvins,
                Fluid.Density.KilogramsPerCubicMeter,
                Fluid.DynamicViscosity?.PascalSeconds,
                Fluid.Enthalpy.JoulesPerKilogram,
                Fluid.Entropy.JoulesPerKilogramKelvin,
                Fluid.FreezingTemperature?.Kelvins,
                Fluid.InternalEnergy.JoulesPerKilogram,
                Fluid.MaxPressure?.Pascals,
                Fluid.MaxTemperature.Kelvins,
                Fluid.MinPressure?.Pascals,
                Fluid.MinTemperature.Kelvins,
                Fluid.MolarMass?.KilogramsPerMole,
                Fluid.Prandtl,
                Fluid.Pressure.Pascals,
                Fluid.Quality?.DecimalFractions,
                Fluid.SoundSpeed?.MetersPerSecond,
                Fluid.SpecificHeat.JoulesPerKilogramKelvin,
                Fluid.SurfaceTension?.NewtonsPerMeter,
                Fluid.Temperature.Kelvins,
                Fluid.TriplePressure?.Pascals,
                Fluid.TripleTemperature?.Kelvins
            };
            var keys = new List<string>
            {
                "Z", "L", "p_critical", "T_critical", "D", "V", "H", "S", "T_freeze", "U", "P_max", "T_max",
                "P_min", "T_min", "M", "Prandtl", "P", "Q", "A", "C", "I", "T", "p_triple", "T_triple"
            };
            for (var i = 0; i < keys.Count; i++)
                if (keys[i] is not ("P" or "T"))
                    actual[i].Should().BeApproximately(HighLevelInterface(keys[i]), 1e-9);
            Fluid.KinematicViscosity.Should().Be(Fluid.DynamicViscosity / Fluid.Density);
        }

        [Test]
        public static void TestUpdateInvalidInput()
        {
            Action action =
                () => Water.Update(Input.Pressure(1.Atmospheres()),
                    Input.Pressure(101325.Pascals()));
            action.Should().Throw<ArgumentException>()
                .WithMessage("Need to define 2 unique inputs!");
        }

        [Test]
        public static void TestCachedInputs()
        {
            var water = Water.WithState(Input.Pressure(101325.Pascals()),
                Input.Temperature(293.15.Kelvins()));
            water.Pressure.Pascals.Should().Be(101325);
            water.Temperature.Kelvins.Should().Be(293.15);
        }

        [Test]
        public static void TestEquals()
        {
            var origin = Water.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            var same = Water.WithState(Input.Pressure(101325.Pascals()),
                Input.Temperature(293.15.Kelvins()));
            var other = Water.WithState(Input.Pressure(1.Atmospheres()),
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
            var origin = Water.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            var same = Water.WithState(Input.Pressure(101325.Pascals()),
                Input.Temperature(293.15.Kelvins()));
            var other = Water.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(30.DegreesCelsius()));
            origin.GetHashCode().Should().Be(same.GetHashCode());
            origin.GetHashCode().Should().NotBe(other.GetHashCode());
        }

        [TestCase(true)]
        [TestCase(false)]
        public static void TestAsJson(bool indented)
        {
            var water = Water.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            water.AsJson(indented).Should().Be(
                JsonConvert.SerializeObject(water, new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>
                        {new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()},
                    Formatting = indented ? Formatting.Indented : Formatting.None
                }));
        }

        [Test]
        public static void TestClone()
        {
            var origin = Water.WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius()));
            var clone = origin.Clone();
            clone.Should().Be(origin);
            clone.Update(Input.Pressure(1.Atmospheres()),
                Input.Temperature(30.DegreesCelsius()));
            clone.Should().NotBe(origin);
        }

        [Test]
        public static void TestDispose() => Water.Factory().Dispose();

        private void SetUp(FluidsList name, Pressure pressure)
        {
            Fraction = SetUpFraction(name);
            Fluid = new Fluid(name, Fraction);
            Pressure = pressure;
            Temperature = SetUpTemperature(Fluid);
            Fluid.Update(Input.Pressure(Pressure), Input.Temperature(Temperature));
        }

        private static Ratio? SetUpFraction(FluidsList name) =>
            name.Pure()
                ? null
                : 0.1 * name.FractionMin() +
                  0.9 * name.FractionMax();

        private static Temperature SetUpTemperature(AbstractFluid fluid) =>
            (0.1 * fluid.MinTemperature.Kelvins +
             0.9 * fluid.MaxTemperature.Kelvins).Kelvins();

        private double? HighLevelInterface(string outputKey)
        {
            try
            {
                double value;
                if (Fraction.HasValue)
                {
                    value = CP.PropsSImulti(new StringVector {outputKey},
                        "P", new DoubleVector {Pressure.Pascals},
                        "T", new DoubleVector {Temperature.Kelvins},
                        Fluid.Name.CoolPropBackend(),
                        new StringVector {Fluid.Name.CoolPropName()},
                        new DoubleVector {Fraction.Value.DecimalFractions})[0][0];
                    return CheckedValue(value, outputKey);
                }

                value = CP.PropsSI(outputKey, "P", Pressure.Pascals, "T", Temperature.Kelvins,
                    CoolPropHighLevelName(Fluid.Name));
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