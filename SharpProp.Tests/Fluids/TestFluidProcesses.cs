using System;
using FluentAssertions;
using NUnit.Framework;
using SharpProp.Extensions;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToSpecificEnergy;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace SharpProp.Tests
{
    public static class TestFluidProcesses
    {
        private static readonly Fluid Water =
            new Fluid(FluidsList.Water).WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(150.DegreesCelsius()));

        private static readonly Pressure HighPressure = 2 * Water.Pressure;
        private static readonly Pressure LowPressure = 0.5 * Water.Pressure;
        private static readonly Ratio IsentropicEfficiency = 80.Percent();
        private static readonly TemperatureDelta TemperatureDelta = TemperatureDelta.FromKelvins(10);
        private static readonly SpecificEnergy EnthalpyDelta = 50.KilojoulesPerKilogram();
        private static readonly Pressure PressureDrop = 50.Kilopascals();

        [Test]
        public static void TestIsentropicCompressionTo() =>
            Water.IsentropicCompressionTo(HighPressure).Should().Be(
                Water.WithState(Input.Pressure(HighPressure),
                    Input.Entropy(Water.Entropy)));

        [Test]
        public static void TestIsentropicCompressionTo_WrongInput()
        {
            Action action = () =>
                _ = Water.IsentropicCompressionTo(LowPressure);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Compressor outlet pressure should be higher than inlet pressure!");
        }

        [Test]
        public static void TestCompressionTo() =>
            Water.CompressionTo(HighPressure, IsentropicEfficiency).Should().Be(
                Water.WithState(Input.Pressure(HighPressure),
                    Input.Enthalpy(Water.Enthalpy + (Water.IsentropicCompressionTo(
                        HighPressure).Enthalpy - Water.Enthalpy) / IsentropicEfficiency.DecimalFractions)));

        [TestCase(0.5, 80, "Compressor outlet pressure should be higher than inlet pressure!")]
        [TestCase(2, 0, "Invalid compressor isentropic efficiency!")]
        [TestCase(2, 100, "Invalid compressor isentropic efficiency!")]
        public static void TestCompressionTo_WrongInput(double pressureRatio,
            double isentropicEfficiency, string message)
        {
            Action action = () =>
                _ = Water.CompressionTo(pressureRatio * Water.Pressure,
                    isentropicEfficiency.Percent());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Test]
        public static void TestIsenthalpicExpansionTo() =>
            Water.IsenthalpicExpansionTo(LowPressure).Should().Be(
                Water.WithState(Input.Pressure(LowPressure),
                    Input.Enthalpy(Water.Enthalpy)));

        [Test]
        public static void TestIsenthalpicExpansionTo_WrongInput()
        {
            Action action = () =>
                _ = Water.IsenthalpicExpansionTo(HighPressure);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Expansion valve outlet pressure should be lower than inlet pressure!");
        }

        [Test]
        public static void TestIsentropicExpansionTo() =>
            Water.IsentropicExpansionTo(LowPressure).Should().Be(
                Water.WithState(Input.Pressure(LowPressure),
                    Input.Entropy(Water.Entropy)));

        [Test]
        public static void TestIsentropicExpansionTo_WrongInput()
        {
            Action action = () =>
                _ = Water.IsentropicExpansionTo(HighPressure);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Expander outlet pressure should be lower than inlet pressure!");
        }

        [Test]
        public static void TestExpansionTo() =>
            Water.ExpansionTo(LowPressure, IsentropicEfficiency).Should().Be(
                Water.WithState(Input.Pressure(LowPressure),
                    Input.Enthalpy(Water.Enthalpy - (Water.Enthalpy - Water.IsentropicExpansionTo(
                        LowPressure).Enthalpy) * IsentropicEfficiency.DecimalFractions)));

        [TestCase(2, 80, "Expander outlet pressure should be lower than inlet pressure!")]
        [TestCase(0.5, 0, "Invalid expander isentropic efficiency!")]
        [TestCase(0.5, 100, "Invalid expander isentropic efficiency!")]
        public static void TestExpansionTo_WrongInput(double pressureRatio,
            double isentropicEfficiency, string message)
        {
            Action action = () =>
                _ = Water.ExpansionTo(pressureRatio * Water.Pressure,
                    isentropicEfficiency.Percent());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Test]
        public static void TestCoolingToTemperature() =>
            Water.CoolingTo(Water.Temperature - TemperatureDelta, PressureDrop)
                .Should().Be(Water.WithState(Input.Pressure(Water.Pressure - PressureDrop),
                    Input.Temperature(Water.Temperature - TemperatureDelta)));

        [TestCase(5, 0, "During the cooling process, the temperature should decrease!")]
        [TestCase(-5, -10, "Invalid pressure drop in the heat exchanger!")]
        public static void TestCoolingToTemperature_WrongInput(double temperatureDelta,
            double pressureDrop, string message)
        {
            Action action = () =>
                _ = Water.CoolingTo(
                    Water.Temperature + TemperatureDelta.FromKelvins(temperatureDelta),
                    pressureDrop.Kilopascals());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Test]
        public static void TestCoolingToEnthalpy() =>
            Water.CoolingTo(Water.Enthalpy - EnthalpyDelta, PressureDrop)
                .Should().Be(Water.WithState(Input.Pressure(Water.Pressure - PressureDrop),
                    Input.Enthalpy(Water.Enthalpy - EnthalpyDelta)));

        [TestCase(5, 0, "During the cooling process, the enthalpy should decrease!")]
        [TestCase(-5, -10, "Invalid pressure drop in the heat exchanger!")]
        public static void TestCoolingToEnthalpy_WrongInput(double enthalpyDelta,
            double pressureDrop, string message)
        {
            Action action = () =>
                _ = Water.CoolingTo(
                    Water.Enthalpy + enthalpyDelta.KilojoulesPerKilogram(),
                    pressureDrop.Kilopascals());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Test]
        public static void TestHeatingToTemperature() =>
            Water.HeatingTo(Water.Temperature + TemperatureDelta, PressureDrop)
                .Should().Be(Water.WithState(Input.Pressure(Water.Pressure - PressureDrop),
                    Input.Temperature(Water.Temperature + TemperatureDelta)));

        [TestCase(5, 0, "During the heating process, the temperature should increase!")]
        [TestCase(-5, -10, "Invalid pressure drop in the heat exchanger!")]
        public static void TestHeatingToTemperature_WrongInput(double temperatureDelta,
            double pressureDrop, string message)
        {
            Action action = () =>
                _ = Water.HeatingTo(
                    Water.Temperature - TemperatureDelta.FromKelvins(temperatureDelta),
                    pressureDrop.Kilopascals());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Test]
        public static void TestHeatingToEnthalpy() =>
            Water.HeatingTo(Water.Enthalpy + EnthalpyDelta, PressureDrop)
                .Should().Be(Water.WithState(Input.Pressure(Water.Pressure - PressureDrop),
                    Input.Enthalpy(Water.Enthalpy + EnthalpyDelta)));

        [TestCase(5, 0, "During the heating process, the enthalpy should increase!")]
        [TestCase(-5, -10, "Invalid pressure drop in the heat exchanger!")]
        public static void TestHeatingToEnthalpy_WrongInput(double enthalpyDelta,
            double pressureDrop, string message)
        {
            Action action = () =>
                _ = Water.HeatingTo(
                    Water.Enthalpy - enthalpyDelta.KilojoulesPerKilogram(),
                    pressureDrop.Kilopascals());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Test]
        public static void TestBubblePointAt() =>
            Water.BubblePointAt(1.Atmospheres()).Should().Be(
                Water.WithState(Input.Pressure(1.Atmospheres()),
                    Input.Quality(0.Percent())));

        [Test]
        public static void TestDewPointAt() =>
            Water.DewPointAt(1.Atmospheres()).Should().Be(
                Water.WithState(Input.Pressure(1.Atmospheres()),
                    Input.Quality(100.Percent())));

        [Test]
        public static void TestTwoPhasePointAt() =>
            Water.TwoPhasePointAt(1.Atmospheres(), 50.Percent()).Should().Be(
                Water.WithState(Input.Pressure(1.Atmospheres()),
                    Input.Quality(50.Percent())));

        [Test]
        public static void TestMixing()
        {
            var first = Water.CoolingTo(Water.Temperature - TemperatureDelta);
            var second = Water.HeatingTo(Water.Temperature + TemperatureDelta);
            Water.Mixing(100.Percent(), first, 200.Percent(), second).Should().Be(
                Water.WithState(Input.Pressure(Water.Pressure),
                    Input.Enthalpy((1 * first.Enthalpy + 2 * second.Enthalpy) / 3.0)));
        }

        [Test]
        public static void TestMixing_WrongFluids()
        {
            var first = new Fluid(FluidsList.Ammonia).DewPointAt(1.Atmospheres());
            var second = Water.HeatingTo(Water.Temperature + TemperatureDelta);
            Action action = () =>
                _ = Water.Mixing(100.Percent(), first, 200.Percent(), second);
            action.Should().Throw<ArgumentException>().WithMessage(
                "The mixing process is possible only for the same fluids!");
        }

        [Test]
        public static void TestMixing_WrongPressures()
        {
            var first = Water.Clone();
            var second = Water.IsentropicCompressionTo(HighPressure);
            Action action = () =>
                _ = Water.Mixing(100.Percent(), first, 200.Percent(), second);
            action.Should().Throw<ArgumentException>().WithMessage(
                "The mixing process is possible only for flows with the same pressure!");
        }
    }
}