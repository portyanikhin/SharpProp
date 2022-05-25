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
        private static Fluid Water =>
            new Fluid(FluidsList.Water).WithState(Input.Pressure(1.Atmospheres()),
                Input.Temperature(150.DegreesCelsius()));

        [Test]
        public static void TestIsentropicCompressionTo() =>
            Water.IsentropicCompressionTo(2 * Water.Pressure).Should().Be(
                Water.WithState(Input.Pressure(2 * Water.Pressure),
                    Input.Entropy(Water.Entropy)));

        [Test]
        public static void TestIsentropicCompressionTo_WrongInput()
        {
            Action action = () =>
                _ = Water.IsentropicCompressionTo(0.5 * Water.Pressure);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Compressor outlet pressure should be higher than inlet pressure!");
        }

        [Test]
        public static void TestCompressionTo() =>
            Water.CompressionTo(2 * Water.Pressure, 80.Percent()).Should().Be(
                Water.WithState(Input.Pressure(2 * Water.Pressure),
                    Input.Enthalpy(Water.Enthalpy + (Water.IsentropicCompressionTo(
                        2 * Water.Pressure).Enthalpy - Water.Enthalpy) / 0.8)));

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
            Water.IsenthalpicExpansionTo(0.5 * Water.Pressure).Should().Be(
                Water.WithState(Input.Pressure(0.5 * Water.Pressure),
                    Input.Enthalpy(Water.Enthalpy)));

        [Test]
        public static void TestIsenthalpicExpansionTo_WrongInput()
        {
            Action action = () =>
                _ = Water.IsenthalpicExpansionTo(2 * Water.Pressure);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Expansion valve outlet pressure should be lower than inlet pressure!");
        }

        [Test]
        public static void TestIsentropicExpansionTo() =>
            Water.IsentropicExpansionTo(0.5 * Water.Pressure).Should().Be(
                Water.WithState(Input.Pressure(0.5 * Water.Pressure),
                    Input.Entropy(Water.Entropy)));

        [Test]
        public static void TestIsentropicExpansionTo_WrongInput()
        {
            Action action = () =>
                _ = Water.IsentropicExpansionTo(2 * Water.Pressure);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Expander outlet pressure should be lower than inlet pressure!");
        }

        [Test]
        public static void TestExpansionTo() =>
            Water.ExpansionTo(0.5 * Water.Pressure, 80.Percent()).Should().Be(
                Water.WithState(Input.Pressure(0.5 * Water.Pressure),
                    Input.Enthalpy(Water.Enthalpy - (Water.Enthalpy - Water.IsentropicExpansionTo(
                        0.5 * Water.Pressure).Enthalpy) * 0.8)));

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
            Water.CoolingTo(Water.Temperature - TemperatureDelta.FromKelvins(10), 50.Kilopascals())
                .Should().Be(
                    Water.WithState(Input.Pressure(Water.Pressure - 50.Kilopascals()),
                        Input.Temperature(Water.Temperature - TemperatureDelta.FromKelvins(10))));

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
            Water.CoolingTo(Water.Enthalpy - 50.KilojoulesPerKilogram(), 50.Kilopascals())
                .Should().Be(
                    Water.WithState(Input.Pressure(Water.Pressure - 50.Kilopascals()),
                        Input.Enthalpy(Water.Enthalpy - 50.KilojoulesPerKilogram())));

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
            Water.HeatingTo(Water.Temperature + TemperatureDelta.FromKelvins(10), 50.Kilopascals())
                .Should().Be(
                    Water.WithState(Input.Pressure(Water.Pressure - 50.Kilopascals()),
                        Input.Temperature(Water.Temperature + TemperatureDelta.FromKelvins(10))));

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
            Water.HeatingTo(Water.Enthalpy + 50.KilojoulesPerKilogram(), 50.Kilopascals())
                .Should().Be(
                    Water.WithState(Input.Pressure(Water.Pressure - 50.Kilopascals()),
                        Input.Enthalpy(Water.Enthalpy + 50.KilojoulesPerKilogram())));

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
            var first = Water.CoolingTo(
                Water.Temperature - TemperatureDelta.FromKelvins(10));
            var second = Water.HeatingTo(
                Water.Temperature + TemperatureDelta.FromKelvins(10));
            Water.Mixing(100.Percent(), first, 200.Percent(), second).Should().Be(
                Water.WithState(
                    Input.Pressure(Water.Pressure),
                    Input.Enthalpy((1 * first.Enthalpy + 2 * second.Enthalpy) / 3.0)));
        }

        [Test]
        public static void TestMixing_WrongFluids()
        {
            var first = new Fluid(FluidsList.Ammonia).DewPointAt(1.Atmospheres());
            var second = Water.HeatingTo(
                Water.Temperature + TemperatureDelta.FromKelvins(10));
            Action action = () =>
                _ = Water.Mixing(100.Percent(), first, 200.Percent(), second);
            action.Should().Throw<ArgumentException>().WithMessage(
                "The mixing process is possible only for the same fluids!");
        }

        [Test]
        public static void TestMixing_WrongPressures()
        {
            var first = Water.Clone();
            var second = Water.IsentropicCompressionTo(2 * Water.Pressure);
            Action action = () =>
                _ = Water.Mixing(100.Percent(), first, 200.Percent(), second);
            action.Should().Throw<ArgumentException>().WithMessage(
                "The mixing process is possible only for flows with the same pressure!");
        }
    }
}