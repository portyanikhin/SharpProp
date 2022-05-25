using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace SharpProp.Tests
{
    public static class TestMixtureProcesses
    {
        private static Mixture Mixture =>
            new Mixture(
                    new List<FluidsList> {FluidsList.Argon, FluidsList.IsoButane},
                    new List<Ratio> {50.Percent(), 50.Percent()})
                .WithState(Input.Pressure(1.Atmospheres()),
                    Input.Temperature(20.DegreesCelsius()));

        [Test]
        public static void TestCoolingToTemperature() =>
            Mixture.CoolingTo(Mixture.Temperature - TemperatureDelta.FromKelvins(10)).Should().Be(
                Mixture.WithState(Input.Pressure(Mixture.Pressure),
                    Input.Temperature(Mixture.Temperature - TemperatureDelta.FromKelvins(10))));

        [TestCase(5, 0, "During the cooling process, the temperature should decrease!")]
        [TestCase(-5, -10, "Invalid pressure drop in the heat exchanger!")]
        public static void TestCoolingToTemperature_WrongInput(double temperatureDelta,
            double pressureDrop, string message)
        {
            Action action = () =>
                _ = Mixture.CoolingTo(
                    Mixture.Temperature + TemperatureDelta.FromKelvins(temperatureDelta),
                    pressureDrop.Kilopascals());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Test]
        public static void TestHeatingToTemperature() =>
            Mixture.HeatingTo(Mixture.Temperature + TemperatureDelta.FromKelvins(10)).Should().Be(
                Mixture.WithState(Input.Pressure(Mixture.Pressure),
                    Input.Temperature(Mixture.Temperature + TemperatureDelta.FromKelvins(10))));

        [TestCase(5, 0, "During the heating process, the temperature should increase!")]
        [TestCase(-5, -10, "Invalid pressure drop in the heat exchanger!")]
        public static void TestHeatingToTemperature_WrongInput(double temperatureDelta,
            double pressureDrop, string message)
        {
            Action action = () =>
                _ = Mixture.HeatingTo(
                    Mixture.Temperature - TemperatureDelta.FromKelvins(temperatureDelta),
                    pressureDrop.Kilopascals());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }
    }
}