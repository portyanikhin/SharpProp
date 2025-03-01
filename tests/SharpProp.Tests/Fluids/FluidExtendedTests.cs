using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

[Collection("Fluids")]
public class FluidExtendedTests : IDisposable
{
    private static readonly Ratio IsentropicEfficiency = 80.Percent();
    private static readonly TemperatureDelta TemperatureDelta = TemperatureDelta.FromKelvins(10);
    private static readonly SpecificEnergy EnthalpyDelta = 50.KilojoulesPerKilogram();

    private readonly IFluidExtended _fluid = new FluidExtended(FluidsList.Water).WithState(
        Input.Pressure(1.Atmospheres()),
        Input.Temperature(20.DegreesCelsius())
    );

    private Pressure HighPressure => 2 * _fluid.Pressure;
    private Pressure LowPressure => 0.5 * _fluid.Pressure;

    public void Dispose()
    {
        _fluid.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void SpecificHeatConstVolume_WaterInStandardConditions_Returns4156() =>
        _fluid
            .SpecificHeatConstVolume.JoulesPerKilogramKelvin.Should()
            .BeApproximately(4156.6814728615545, 1e-6);

    [Fact]
    public void MolarDensity_WaterInStandardConditions_Returns55408() =>
        _fluid.MolarDensity?.KilogramsPerMole.Should().BeApproximately(55408.953697937126, 1e-6);

    [Fact]
    public void OzoneDepletionPotential_Water_ReturnsNull() =>
        _fluid.OzoneDepletionPotential.Should().BeNull();

    [Fact]
    public void Methods_New_ReturnsInstancesOfInheritedType()
    {
        _fluid.Clone().Should().BeOfType<FluidExtended>();
        _fluid.Factory().Should().BeOfType<FluidExtended>();
        _fluid.SpecifyPhase(Phases.Gas).UnspecifyPhase().Should().BeOfType<FluidExtended>();
        _fluid
            .WithState(Input.Pressure(HighPressure), Input.Temperature(_fluid.Temperature))
            .Should()
            .BeOfType<FluidExtended>();
        _fluid.IsentropicCompressionTo(HighPressure).Should().BeOfType<FluidExtended>();
        _fluid.CompressionTo(HighPressure, IsentropicEfficiency).Should().BeOfType<FluidExtended>();
        _fluid.IsenthalpicExpansionTo(LowPressure).Should().BeOfType<FluidExtended>();
        _fluid.IsentropicExpansionTo(LowPressure).Should().BeOfType<FluidExtended>();
        _fluid.ExpansionTo(LowPressure, IsentropicEfficiency).Should().BeOfType<FluidExtended>();
        _fluid.CoolingTo(_fluid.Temperature - TemperatureDelta).Should().BeOfType<FluidExtended>();
        _fluid.CoolingTo(_fluid.Enthalpy - EnthalpyDelta).Should().BeOfType<FluidExtended>();
        _fluid.HeatingTo(_fluid.Temperature + TemperatureDelta).Should().BeOfType<FluidExtended>();
        _fluid.HeatingTo(_fluid.Enthalpy + EnthalpyDelta).Should().BeOfType<FluidExtended>();
        _fluid.BubblePointAt(1.Atmospheres()).Should().BeOfType<FluidExtended>();
        _fluid.BubblePointAt(100.DegreesCelsius()).Should().BeOfType<FluidExtended>();
        _fluid.DewPointAt(1.Atmospheres()).Should().BeOfType<FluidExtended>();
        _fluid.DewPointAt(100.DegreesCelsius()).Should().BeOfType<FluidExtended>();
        _fluid.TwoPhasePointAt(1.Atmospheres(), 50.Percent()).Should().BeOfType<FluidExtended>();
        _fluid
            .Mixing(
                100.Percent(),
                _fluid.CoolingTo(_fluid.Temperature - TemperatureDelta),
                200.Percent(),
                _fluid.HeatingTo(_fluid.Temperature + TemperatureDelta)
            )
            .Should()
            .BeOfType<FluidExtended>();
    }
}
