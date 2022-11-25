using System;
using CoolProp;
using FluentAssertions;
using UnitsNet;
using UnitsNet.NumberExtensions.NumberToPressure;
using UnitsNet.NumberExtensions.NumberToRatio;
using UnitsNet.NumberExtensions.NumberToSpecificEnergy;
using UnitsNet.NumberExtensions.NumberToTemperature;
using UnitsNet.Units;
using Xunit;

namespace SharpProp.Tests
{
    /// <summary>
    ///     An example of how to add new properties to the <see cref="Fluid" /> class.
    /// </summary>
    public class FluidExtended : Fluid
    {
        private MolarMass? _molarDensity;
        private double? _ozoneDepletionPotential;
        private SpecificEntropy? _specificHeatConstVolume;

        public FluidExtended(FluidsList name, Ratio? fraction = null) :
            base(name, fraction)
        {
        }

        /// <summary>
        ///     Mass specific constant volume specific heat (by default, kJ/kg/K).
        /// </summary>
        public SpecificEntropy SpecificHeatConstVolume => _specificHeatConstVolume ??=
            SpecificEntropy.FromJoulesPerKilogramKelvin(KeyedOutput(Parameters.iCvmass))
                .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

        /// <summary>
        ///     Molar density (by default, kg/mol).
        /// </summary>
        public MolarMass? MolarDensity => _molarDensity ??=
            KeyedOutputIsNotNull(Parameters.iDmolar, out var output)
                ? UnitsNet.MolarMass.FromKilogramsPerMole(output!.Value)
                : null;

        /// <summary>
        ///     Ozone depletion potential (ODP).
        /// </summary>
        public double? OzoneDepletionPotential =>
            _ozoneDepletionPotential ??= NullableKeyedOutput(Parameters.iODP);

        protected override void Reset()
        {
            base.Reset();
            _specificHeatConstVolume = null;
            _molarDensity = null;
            _ozoneDepletionPotential = null;
        }

        public override FluidExtended Factory() => new(Name, Fraction);

        public override FluidExtended WithState(IKeyedInput<Parameters> firstInput,
            IKeyedInput<Parameters> secondInput) =>
            (FluidExtended) base.WithState(firstInput, secondInput);

        public override FluidExtended IsentropicCompressionTo(Pressure pressure) =>
            (FluidExtended) base.IsentropicCompressionTo(pressure);

        public override FluidExtended CompressionTo(Pressure pressure, Ratio isentropicEfficiency) =>
            (FluidExtended) base.CompressionTo(pressure, isentropicEfficiency);

        public override FluidExtended IsenthalpicExpansionTo(Pressure pressure) =>
            (FluidExtended) base.IsenthalpicExpansionTo(pressure);

        public override FluidExtended IsentropicExpansionTo(Pressure pressure) =>
            (FluidExtended) base.IsentropicExpansionTo(pressure);

        public override FluidExtended ExpansionTo(Pressure pressure,
            Ratio isentropicEfficiency) =>
            (FluidExtended) base.ExpansionTo(pressure, isentropicEfficiency);

        public override FluidExtended CoolingTo(Temperature temperature,
            Pressure? pressureDrop = null) =>
            (FluidExtended) base.CoolingTo(temperature, pressureDrop);

        public override FluidExtended CoolingTo(SpecificEnergy enthalpy,
            Pressure? pressureDrop = null) =>
            (FluidExtended) base.CoolingTo(enthalpy, pressureDrop);

        public override FluidExtended HeatingTo(Temperature temperature,
            Pressure? pressureDrop = null) =>
            (FluidExtended) base.HeatingTo(temperature, pressureDrop);

        public override FluidExtended HeatingTo(SpecificEnergy enthalpy,
            Pressure? pressureDrop = null) =>
            (FluidExtended) base.HeatingTo(enthalpy, pressureDrop);

        public override FluidExtended BubblePointAt(Pressure pressure) =>
            (FluidExtended) base.BubblePointAt(pressure);

        public override FluidExtended BubblePointAt(Temperature temperature) =>
            (FluidExtended) base.BubblePointAt(temperature);

        public override FluidExtended DewPointAt(Pressure pressure) =>
            (FluidExtended) base.DewPointAt(pressure);

        public override FluidExtended DewPointAt(Temperature temperature) =>
            (FluidExtended) base.DewPointAt(temperature);

        public override FluidExtended TwoPhasePointAt(Pressure pressure, Ratio quality) =>
            (FluidExtended) base.TwoPhasePointAt(pressure, quality);

        public override FluidExtended Mixing(Ratio firstSpecificMassFlow, AbstractFluid first,
            Ratio secondSpecificMassFlow, AbstractFluid second) =>
            (FluidExtended) base.Mixing(firstSpecificMassFlow, first,
                secondSpecificMassFlow, second);
    }

    [Collection("Fluids")]
    public class FluidExtendedTests : IDisposable
    {
        private static readonly Ratio IsentropicEfficiency = 80.Percent();
        private static readonly TemperatureDelta TemperatureDelta = TemperatureDelta.FromKelvins(10);
        private static readonly SpecificEnergy EnthalpyDelta = 50.KilojoulesPerKilogram();

        public FluidExtendedTests() =>
            Fluid = new FluidExtended(FluidsList.Water)
                .WithState(Input.Pressure(1.Atmospheres()),
                    Input.Temperature(20.DegreesCelsius()));

        private FluidExtended Fluid { get; }
        private Pressure HighPressure => 2 * Fluid.Pressure;
        private Pressure LowPressure => 0.5 * Fluid.Pressure;

        public void Dispose()
        {
            Fluid.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void SpecificHeatConstVolume_WaterInStandardConditions_Returns4156() =>
            Fluid.SpecificHeatConstVolume.JoulesPerKilogramKelvin
                .Should().Be(4156.6814728615545);

        [Fact]
        public void MolarDensity_WaterInStandardConditions_Returns55408() =>
            Fluid.MolarDensity?.KilogramsPerMole
                .Should().Be(55408.953697937126);

        [Fact]
        public void OzoneDepletionPotential_Water_ReturnsNull() =>
            Fluid.OzoneDepletionPotential.Should().BeNull();

        [Fact]
        public void Processes_Override_ReturnsInstancesOfInheritedType()
        {
            Fluid.IsentropicCompressionTo(HighPressure)
                .Should().BeOfType<FluidExtended>();
            Fluid.CompressionTo(HighPressure, IsentropicEfficiency)
                .Should().BeOfType<FluidExtended>();
            Fluid.IsenthalpicExpansionTo(LowPressure)
                .Should().BeOfType<FluidExtended>();
            Fluid.IsentropicExpansionTo(LowPressure)
                .Should().BeOfType<FluidExtended>();
            Fluid.ExpansionTo(LowPressure, IsentropicEfficiency)
                .Should().BeOfType<FluidExtended>();
            Fluid.CoolingTo(Fluid.Temperature - TemperatureDelta)
                .Should().BeOfType<FluidExtended>();
            Fluid.CoolingTo(Fluid.Enthalpy - EnthalpyDelta)
                .Should().BeOfType<FluidExtended>();
            Fluid.HeatingTo(Fluid.Temperature + TemperatureDelta)
                .Should().BeOfType<FluidExtended>();
            Fluid.HeatingTo(Fluid.Enthalpy + EnthalpyDelta)
                .Should().BeOfType<FluidExtended>();
            Fluid.BubblePointAt(1.Atmospheres())
                .Should().BeOfType<FluidExtended>();
            Fluid.BubblePointAt(100.DegreesCelsius())
                .Should().BeOfType<FluidExtended>();
            Fluid.DewPointAt(1.Atmospheres())
                .Should().BeOfType<FluidExtended>();
            Fluid.DewPointAt(100.DegreesCelsius())
                .Should().BeOfType<FluidExtended>();
            Fluid.TwoPhasePointAt(1.Atmospheres(), 50.Percent())
                .Should().BeOfType<FluidExtended>();
            Fluid.Mixing(
                    100.Percent(),
                    Fluid.CoolingTo(Fluid.Temperature - TemperatureDelta),
                    200.Percent(),
                    Fluid.HeatingTo(Fluid.Temperature + TemperatureDelta))
                .Should().BeOfType<FluidExtended>();
        }
    }
}