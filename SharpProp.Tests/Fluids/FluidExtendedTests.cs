using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp.Tests;

/// <summary>
///     An example of how to
///     add new properties to the
///     <see cref="Fluid"/> class.
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
    ///     Mass specific constant
    ///     volume specific heat
    ///     (by default, kJ/kg/K).
    /// </summary>
    public SpecificEntropy SpecificHeatConstVolume =>
        _specificHeatConstVolume ??=
            SpecificEntropy.FromJoulesPerKilogramKelvin(
                KeyedOutput(Parameters.iCvmass)
            ).ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

    /// <summary>
    ///     Molar density
    ///     (by default, kg/mol).
    /// </summary>
    public MolarMass? MolarDensity => _molarDensity ??=
        KeyedOutputIsNotNull(Parameters.iDmolar, out var output)
            ? UnitsNet.MolarMass.FromKilogramsPerMole(output!.Value)
            : null;

    /// <summary>
    ///     Ozone depletion potential (ODP).
    /// </summary>
    public double? OzoneDepletionPotential =>
        _ozoneDepletionPotential ??=
            NullableKeyedOutput(Parameters.iODP);

    protected override void Reset()
    {
        base.Reset();
        _specificHeatConstVolume = null;
        _molarDensity = null;
        _ozoneDepletionPotential = null;
    }

    protected override AbstractFluid CreateInstance() =>
        new FluidExtended(Name, Fraction);

    public new FluidExtended Clone() =>
        (FluidExtended) base.Clone();

    public new FluidExtended Factory() =>
        (FluidExtended) base.Factory();

    public new FluidExtended WithState(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    ) => (FluidExtended) base.WithState(firstInput, secondInput);

    public new FluidExtended IsentropicCompressionTo(Pressure pressure) =>
        (FluidExtended) base.IsentropicCompressionTo(pressure);

    public new FluidExtended CompressionTo(
        Pressure pressure,
        Ratio isentropicEfficiency
    ) => (FluidExtended) base.CompressionTo(pressure, isentropicEfficiency);

    public new FluidExtended IsenthalpicExpansionTo(Pressure pressure) =>
        (FluidExtended) base.IsenthalpicExpansionTo(pressure);

    public new FluidExtended IsentropicExpansionTo(Pressure pressure) =>
        (FluidExtended) base.IsentropicExpansionTo(pressure);

    public new FluidExtended ExpansionTo(
        Pressure pressure,
        Ratio isentropicEfficiency
    ) => (FluidExtended) base.ExpansionTo(pressure, isentropicEfficiency);

    public new FluidExtended CoolingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    ) => (FluidExtended) base.CoolingTo(temperature, pressureDrop);

    public new FluidExtended CoolingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    ) => (FluidExtended) base.CoolingTo(enthalpy, pressureDrop);

    public new FluidExtended HeatingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    ) => (FluidExtended) base.HeatingTo(temperature, pressureDrop);

    public new FluidExtended HeatingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    ) => (FluidExtended) base.HeatingTo(enthalpy, pressureDrop);

    public new FluidExtended BubblePointAt(Pressure pressure) =>
        (FluidExtended) base.BubblePointAt(pressure);

    public new FluidExtended BubblePointAt(Temperature temperature) =>
        (FluidExtended) base.BubblePointAt(temperature);

    public new FluidExtended DewPointAt(Pressure pressure) =>
        (FluidExtended) base.DewPointAt(pressure);

    public new FluidExtended DewPointAt(Temperature temperature) =>
        (FluidExtended) base.DewPointAt(temperature);

    public new FluidExtended TwoPhasePointAt(
        Pressure pressure,
        Ratio quality
    ) => (FluidExtended) base.TwoPhasePointAt(pressure, quality);

    public new FluidExtended Mixing(
        Ratio firstSpecificMassFlow,
        AbstractFluid first,
        Ratio secondSpecificMassFlow,
        AbstractFluid second
    ) => (FluidExtended) base.Mixing(firstSpecificMassFlow,
        first,
        secondSpecificMassFlow,
        second
    );
}

[Collection("Fluids")]
public class FluidExtendedTests : IDisposable
{
    private static readonly Ratio IsentropicEfficiency =
        80.Percent();

    private static readonly TemperatureDelta TemperatureDelta =
        TemperatureDelta.FromKelvins(10);

    private static readonly SpecificEnergy EnthalpyDelta =
        50.KilojoulesPerKilogram();

    private readonly FluidExtended _fluid;

    public FluidExtendedTests() =>
        _fluid = new FluidExtended(FluidsList.Water)
            .WithState(
                Input.Pressure(1.Atmospheres()),
                Input.Temperature(20.DegreesCelsius())
            );

    private Pressure HighPressure =>
        2 * _fluid.Pressure;

    private Pressure LowPressure =>
        0.5 * _fluid.Pressure;

    public void Dispose()
    {
        _fluid.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void SpecificHeatConstVolume_WaterInStandardConditions_Returns4156() =>
        _fluid.SpecificHeatConstVolume.JoulesPerKilogramKelvin
            .Should().Be(4156.6814728615545);

    [Fact]
    public void MolarDensity_WaterInStandardConditions_Returns55408() =>
        _fluid.MolarDensity?.KilogramsPerMole
            .Should().Be(55408.953697937126);

    [Fact]
    public void OzoneDepletionPotential_Water_ReturnsNull() =>
        _fluid.OzoneDepletionPotential.Should().BeNull();

    [Fact]
    public void Methods_New_ReturnsInstancesOfInheritedType()
    {
        _fluid.Clone()
            .Should().BeOfType<FluidExtended>();
        _fluid.Factory()
            .Should().BeOfType<FluidExtended>();
        _fluid.IsentropicCompressionTo(HighPressure)
            .Should().BeOfType<FluidExtended>();
        _fluid.CompressionTo(HighPressure, IsentropicEfficiency)
            .Should().BeOfType<FluidExtended>();
        _fluid.IsenthalpicExpansionTo(LowPressure)
            .Should().BeOfType<FluidExtended>();
        _fluid.IsentropicExpansionTo(LowPressure)
            .Should().BeOfType<FluidExtended>();
        _fluid.ExpansionTo(LowPressure, IsentropicEfficiency)
            .Should().BeOfType<FluidExtended>();
        _fluid.CoolingTo(_fluid.Temperature - TemperatureDelta)
            .Should().BeOfType<FluidExtended>();
        _fluid.CoolingTo(_fluid.Enthalpy - EnthalpyDelta)
            .Should().BeOfType<FluidExtended>();
        _fluid.HeatingTo(_fluid.Temperature + TemperatureDelta)
            .Should().BeOfType<FluidExtended>();
        _fluid.HeatingTo(_fluid.Enthalpy + EnthalpyDelta)
            .Should().BeOfType<FluidExtended>();
        _fluid.BubblePointAt(1.Atmospheres())
            .Should().BeOfType<FluidExtended>();
        _fluid.BubblePointAt(100.DegreesCelsius())
            .Should().BeOfType<FluidExtended>();
        _fluid.DewPointAt(1.Atmospheres())
            .Should().BeOfType<FluidExtended>();
        _fluid.DewPointAt(100.DegreesCelsius())
            .Should().BeOfType<FluidExtended>();
        _fluid.TwoPhasePointAt(1.Atmospheres(), 50.Percent())
            .Should().BeOfType<FluidExtended>();
        _fluid.Mixing(
            100.Percent(),
            _fluid.CoolingTo(_fluid.Temperature - TemperatureDelta),
            200.Percent(),
            _fluid.HeatingTo(_fluid.Temperature + TemperatureDelta)
        ).Should().BeOfType<FluidExtended>();
    }
}