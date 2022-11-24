using System;
using FluentAssertions;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToSpecificEnergy;
using UnitsNet.NumberExtensions.NumberToTemperature;
using Xunit;

namespace SharpProp.Tests
{
    [Collection("Fluids")]
    public class FluidProcessesTests : IDisposable
    {
        private static readonly Ratio IsentropicEfficiency = 80.Percent();
        private static readonly TemperatureDelta TemperatureDelta = TemperatureDelta.FromKelvins(10);
        private static readonly SpecificEnergy EnthalpyDelta = 50.KilojoulesPerKilogram();
        private static readonly Pressure PressureDrop = 50.Kilopascals();

        public FluidProcessesTests() =>
            Fluid = new Fluid(FluidsList.Water)
                .WithState(Input.Pressure(1.Atmospheres()),
                    Input.Temperature(150.DegreesCelsius()));

        private Fluid Fluid { get; }
        private Pressure HighPressure => 2 * Fluid.Pressure;
        private Pressure LowPressure => 0.5 * Fluid.Pressure;

        public void Dispose()
        {
            Fluid.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void IsentropicCompressionTo_WrongPressure_ThrowsArgumentException()
        {
            Action action = () =>
                _ = Fluid.IsentropicCompressionTo(LowPressure);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Compressor outlet pressure should be higher than inlet pressure!");
        }

        [Fact]
        public void IsentropicCompressionTo_HighPressure_ReturnsFluidAtHighPressureAndSameEntropy() =>
            Fluid.IsentropicCompressionTo(HighPressure).Should().Be(
                Fluid.WithState(Input.Pressure(HighPressure),
                    Input.Entropy(Fluid.Entropy)));

        [Theory]
        [InlineData(0.5, 80, "Compressor outlet pressure should be higher than inlet pressure!")]
        [InlineData(2, 0, "Invalid compressor isentropic efficiency!")]
        [InlineData(2, 100, "Invalid compressor isentropic efficiency!")]
        public void CompressionTo_WrongPressureOrIsentropicEfficiency_ThrowsArgumentException(
            double pressureRatio, double isentropicEfficiency, string message)
        {
            Action action = () =>
                _ = Fluid.CompressionTo(pressureRatio * Fluid.Pressure,
                    isentropicEfficiency.Percent());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Fact]
        public void CompressionTo_HighPressureWithIsentropicEfficiency_ReturnsFluidAtHighPressureAndHigherEntropy() =>
            Fluid.CompressionTo(HighPressure, IsentropicEfficiency).Should().Be(
                Fluid.WithState(Input.Pressure(HighPressure),
                    Input.Enthalpy(Fluid.Enthalpy + (Fluid.IsentropicCompressionTo(
                        HighPressure).Enthalpy - Fluid.Enthalpy) / IsentropicEfficiency.DecimalFractions)));

        [Fact]
        public void IsenthalpicExpansionTo_WrongPressure_ThrowsArgumentException()
        {
            Action action = () =>
                _ = Fluid.IsenthalpicExpansionTo(HighPressure);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Expansion valve outlet pressure should be lower than inlet pressure!");
        }

        [Fact]
        public void IsenthalpicExpansionTo_LowPressure_ReturnsFluidAtLowPressureAndSameEnthalpy() =>
            Fluid.IsenthalpicExpansionTo(LowPressure).Should().Be(
                Fluid.WithState(Input.Pressure(LowPressure),
                    Input.Enthalpy(Fluid.Enthalpy)));

        [Fact]
        public void IsentropicExpansionTo_WrongPressure_ThrowsArgumentException()
        {
            Action action = () =>
                _ = Fluid.IsentropicExpansionTo(HighPressure);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Expander outlet pressure should be lower than inlet pressure!");
        }

        [Fact]
        public void IsentropicExpansionTo_LowPressure_ReturnsFluidAtLowPressureAndSameEntropy() =>
            Fluid.IsentropicExpansionTo(LowPressure).Should().Be(
                Fluid.WithState(Input.Pressure(LowPressure),
                    Input.Entropy(Fluid.Entropy)));

        [Theory]
        [InlineData(2, 80, "Expander outlet pressure should be lower than inlet pressure!")]
        [InlineData(0.5, 0, "Invalid expander isentropic efficiency!")]
        [InlineData(0.5, 100, "Invalid expander isentropic efficiency!")]
        public void ExpansionTo_WrongPressureOrIsentropicEfficiency_ThrowsArgumentException(
            double pressureRatio, double isentropicEfficiency, string message)
        {
            Action action = () =>
                _ = Fluid.ExpansionTo(pressureRatio * Fluid.Pressure,
                    isentropicEfficiency.Percent());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Fact]
        public void ExpansionTo_LowPressureWithIsentropicEfficiency_ReturnsFluidAtLowPressureAndHigherEntropy() =>
            Fluid.ExpansionTo(LowPressure, IsentropicEfficiency).Should().Be(
                Fluid.WithState(Input.Pressure(LowPressure),
                    Input.Enthalpy(Fluid.Enthalpy - (Fluid.Enthalpy - Fluid.IsentropicExpansionTo(
                        LowPressure).Enthalpy) * IsentropicEfficiency.DecimalFractions)));

        [Theory]
        [InlineData(5, 0, "During the cooling process, the temperature should decrease!")]
        [InlineData(-5, -10, "Invalid pressure drop in the heat exchanger!")]
        public void CoolingTo_WrongTemperatureOrPressureDrop_ThrowsArgumentException(
            double temperatureDelta, double pressureDrop, string message)
        {
            Action action = () =>
                _ = Fluid.CoolingTo(
                    Fluid.Temperature + TemperatureDelta.FromKelvins(temperatureDelta),
                    pressureDrop.Kilopascals());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Fact]
        public void CoolingTo_TemperatureWithPressureDrop_ReturnsFluidAtTemperatureAndLowerPressure() =>
            Fluid.CoolingTo(Fluid.Temperature - TemperatureDelta, PressureDrop)
                .Should().Be(Fluid.WithState(Input.Pressure(Fluid.Pressure - PressureDrop),
                    Input.Temperature(Fluid.Temperature - TemperatureDelta)));

        [Theory]
        [InlineData(5, 0, "During the cooling process, the enthalpy should decrease!")]
        [InlineData(-5, -10, "Invalid pressure drop in the heat exchanger!")]
        public void CoolingTo_WrongEnthalpyOrPressureDrop_ThrowsArgumentException(
            double enthalpyDelta, double pressureDrop, string message)
        {
            Action action = () =>
                _ = Fluid.CoolingTo(
                    Fluid.Enthalpy + enthalpyDelta.KilojoulesPerKilogram(),
                    pressureDrop.Kilopascals());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Fact]
        public void CoolingTo_EnthalpyWithPressureDrop_ReturnsFluidAtEnthalpyAndLowerPressure() =>
            Fluid.CoolingTo(Fluid.Enthalpy - EnthalpyDelta, PressureDrop)
                .Should().Be(Fluid.WithState(Input.Pressure(Fluid.Pressure - PressureDrop),
                    Input.Enthalpy(Fluid.Enthalpy - EnthalpyDelta)));

        [Theory]
        [InlineData(5, 0, "During the heating process, the temperature should increase!")]
        [InlineData(-5, -10, "Invalid pressure drop in the heat exchanger!")]
        public void HeatingTo_WrongTemperatureOrPressureDrop_ThrowsArgumentException(
            double temperatureDelta, double pressureDrop, string message)
        {
            Action action = () =>
                _ = Fluid.HeatingTo(
                    Fluid.Temperature - TemperatureDelta.FromKelvins(temperatureDelta),
                    pressureDrop.Kilopascals());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Fact]
        public void HeatingTo_TemperatureWithPressureDrop_ReturnsFluidAtTemperatureAndLowerPressure() =>
            Fluid.HeatingTo(Fluid.Temperature + TemperatureDelta, PressureDrop)
                .Should().Be(Fluid.WithState(Input.Pressure(Fluid.Pressure - PressureDrop),
                    Input.Temperature(Fluid.Temperature + TemperatureDelta)));

        [Theory]
        [InlineData(5, 0, "During the heating process, the enthalpy should increase!")]
        [InlineData(-5, -10, "Invalid pressure drop in the heat exchanger!")]
        public void HeatingTo_WrongEnthalpyOrPressureDrop_ThrowsArgumentException(
            double enthalpyDelta, double pressureDrop, string message)
        {
            Action action = () =>
                _ = Fluid.HeatingTo(
                    Fluid.Enthalpy - enthalpyDelta.KilojoulesPerKilogram(),
                    pressureDrop.Kilopascals());
            action.Should().Throw<ArgumentException>().WithMessage($"*{message}*");
        }

        [Fact]
        public void HeatingTo_EnthalpyWithPressureDrop_ReturnsFluidAtEnthalpyAndLowerPressure() =>
            Fluid.HeatingTo(Fluid.Enthalpy + EnthalpyDelta, PressureDrop)
                .Should().Be(Fluid.WithState(Input.Pressure(Fluid.Pressure - PressureDrop),
                    Input.Enthalpy(Fluid.Enthalpy + EnthalpyDelta)));

        [Fact]
        public void BubblePointAt_Pressure_ReturnsSaturatedLiquidAtPressure() =>
            Fluid.BubblePointAt(1.Atmospheres()).Should().Be(
                Fluid.WithState(Input.Pressure(1.Atmospheres()),
                    Input.Quality(0.Percent())));

        [Fact]
        public void BubblePointAt_Temperature_ReturnsSaturatedLiquidAtTemperature() =>
            Fluid.BubblePointAt(100.DegreesCelsius()).Should().Be(
                Fluid.WithState(Input.Temperature(100.DegreesCelsius()),
                    Input.Quality(0.Percent())));

        [Fact]
        public void DewPointAt_Pressure_ReturnsSaturatedVaporAtPressure() =>
            Fluid.DewPointAt(1.Atmospheres()).Should().Be(
                Fluid.WithState(Input.Pressure(1.Atmospheres()),
                    Input.Quality(100.Percent())));

        [Fact]
        public void DewPointAt_Temperature_ReturnsSaturatedVaporAtTemperature() =>
            Fluid.DewPointAt(100.DegreesCelsius()).Should().Be(
                Fluid.WithState(Input.Temperature(100.DegreesCelsius()),
                    Input.Quality(100.Percent())));

        [Fact]
        public void TwoPhasePointAt_PressureAndQuality_ReturnsFluidAtPressureAndQuality() =>
            Fluid.TwoPhasePointAt(1.Atmospheres(), 50.Percent()).Should().Be(
                Fluid.WithState(Input.Pressure(1.Atmospheres()),
                    Input.Quality(50.Percent())));

        [Fact]
        public void Mixing_WrongFluids_ThrowsArgumentException()
        {
            var first = new Fluid(FluidsList.Ammonia).DewPointAt(1.Atmospheres());
            var second = Fluid.HeatingTo(Fluid.Temperature + TemperatureDelta);
            Action action = () =>
                _ = Fluid.Mixing(100.Percent(), first, 200.Percent(), second);
            action.Should().Throw<ArgumentException>().WithMessage(
                "The mixing process is possible only for the same fluids!");
        }

        [Fact]
        public void Mixing_WrongPressures_ThrowsArgumentException()
        {
            var first = Fluid.Clone();
            var second = Fluid.IsentropicCompressionTo(HighPressure);
            Action action = () =>
                _ = Fluid.Mixing(100.Percent(), first, 200.Percent(), second);
            action.Should().Throw<ArgumentException>().WithMessage(
                "The mixing process is possible only for flows with the same pressure!");
        }

        [Fact]
        public void Mixing_SamePressures_ReturnsMixPoint()
        {
            var first = Fluid.CoolingTo(Fluid.Temperature - TemperatureDelta);
            var second = Fluid.HeatingTo(Fluid.Temperature + TemperatureDelta);
            Fluid.Mixing(100.Percent(), first, 200.Percent(), second).Should().Be(
                Fluid.WithState(Input.Pressure(Fluid.Pressure),
                    Input.Enthalpy((1 * first.Enthalpy + 2 * second.Enthalpy) / 3.0)));
        }
    }
}