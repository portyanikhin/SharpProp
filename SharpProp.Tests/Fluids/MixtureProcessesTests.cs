using System;
using System.Collections.Generic;
using FluentAssertions;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToTemperature;
using Xunit;

namespace SharpProp.Tests
{
    [Collection("Fluids")]
    public class MixtureProcessesTests : IDisposable
    {
        private static readonly TemperatureDelta TemperatureDelta = TemperatureDelta.FromKelvins(10);
        private static readonly Pressure PressureDrop = 50.Kilopascals();

        public MixtureProcessesTests() =>
            Mixture = new Mixture(
                    new List<FluidsList> {FluidsList.Argon, FluidsList.IsoButane},
                    new List<Ratio> {50.Percent(), 50.Percent()})
                .WithState(Input.Pressure(1.Atmospheres()),
                    Input.Temperature(20.DegreesCelsius()));

        private Mixture Mixture { get; }

        public void Dispose()
        {
            Mixture.Dispose();
            GC.SuppressFinalize(this);
        }

        [Theory]
        [InlineData(5, 0, "During the cooling process, the temperature should decrease!")]
        [InlineData(-5, -10, "Invalid pressure drop in the heat exchanger!")]
        public void CoolingTo_WrongTemperatureOrPressureDrop_ThrowsArgumentException(
            double temperatureDelta, double pressureDrop, string message)
        {
            Action action = () =>
                _ = Mixture.CoolingTo(
                    Mixture.Temperature + TemperatureDelta.FromKelvins(temperatureDelta),
                    pressureDrop.Kilopascals());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Fact]
        public void CoolingTo_TemperatureWithPressureDrop_ReturnsMixtureAtTemperatureAndLowerPressure() =>
            Mixture.CoolingTo(Mixture.Temperature - TemperatureDelta, PressureDrop)
                .Should().Be(Mixture.WithState(Input.Pressure(Mixture.Pressure - PressureDrop),
                    Input.Temperature(Mixture.Temperature - TemperatureDelta)));

        [Theory]
        [InlineData(5, 0, "During the heating process, the temperature should increase!")]
        [InlineData(-5, -10, "Invalid pressure drop in the heat exchanger!")]
        public void HeatingTo_WrongTemperatureOrPressureDrop_ThrowsArgumentException(
            double temperatureDelta, double pressureDrop, string message)
        {
            Action action = () =>
                _ = Mixture.HeatingTo(
                    Mixture.Temperature - TemperatureDelta.FromKelvins(temperatureDelta),
                    pressureDrop.Kilopascals());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Fact]
        public void HeatingTo_TemperatureWithPressureDrop_ReturnsMixtureAtTemperatureAndLowerPressure() =>
            Mixture.HeatingTo(Mixture.Temperature + TemperatureDelta, PressureDrop)
                .Should().Be(Mixture.WithState(Input.Pressure(Mixture.Pressure - PressureDrop),
                    Input.Temperature(Mixture.Temperature + TemperatureDelta)));
    }
}