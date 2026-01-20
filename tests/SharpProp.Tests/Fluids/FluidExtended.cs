namespace SharpProp.Tests;

/// <summary>
///     An example of how to add new properties to the <see cref="Fluid"/> class.
/// </summary>
/// <seealso cref="IFluidExtended"/>
public class FluidExtended(FluidsList name, Ratio? fraction = null, string? coolPropBackend = null)
    : Fluid(name, fraction, coolPropBackend),
        IFluidExtended
{
    private MolarMass? _molarDensity;
    private double? _ozoneDepletionPotential;
    private SpecificEntropy? _specificHeatConstVolume;

    public SpecificEntropy SpecificHeatConstVolume =>
        _specificHeatConstVolume ??= SpecificEntropy
            .FromJoulesPerKilogramKelvin(KeyedOutput(Parameters.iCvmass))
            .ToUnit(SpecificEntropyUnit.KilojoulePerKilogramKelvin);

    public MolarMass? MolarDensity =>
        _molarDensity ??= KeyedOutputIsNotNull(Parameters.iDmolar, out var output)
            ? UnitsNet.MolarMass.FromKilogramsPerMole(output!.Value)
            : null;

    public double? OzoneDepletionPotential =>
        _ozoneDepletionPotential ??= NullableKeyedOutput(Parameters.iODP);

    public override void Reset()
    {
        base.Reset();
        _specificHeatConstVolume = null;
        _molarDensity = null;
        _ozoneDepletionPotential = null;
    }

    public new IFluidExtended SpecifyPhase(Phases phase) => (FluidExtended)base.SpecifyPhase(phase);

    public new IFluidExtended UnspecifyPhase() => (FluidExtended)base.UnspecifyPhase();

    public new IFluidExtended WithState(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    ) => (FluidExtended)base.WithState(firstInput, secondInput);

    public new IFluidExtended IsentropicCompressionTo(Pressure pressure) =>
        (FluidExtended)base.IsentropicCompressionTo(pressure);

    public new IFluidExtended CompressionTo(Pressure pressure, Ratio isentropicEfficiency) =>
        (FluidExtended)base.CompressionTo(pressure, isentropicEfficiency);

    public new IFluidExtended IsenthalpicExpansionTo(Pressure pressure) =>
        (FluidExtended)base.IsenthalpicExpansionTo(pressure);

    public new IFluidExtended IsentropicExpansionTo(Pressure pressure) =>
        (FluidExtended)base.IsentropicExpansionTo(pressure);

    public new IFluidExtended ExpansionTo(Pressure pressure, Ratio isentropicEfficiency) =>
        (FluidExtended)base.ExpansionTo(pressure, isentropicEfficiency);

    public new IFluidExtended CoolingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        (FluidExtended)base.CoolingTo(temperature, pressureDrop);

    public new IFluidExtended CoolingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null) =>
        (FluidExtended)base.CoolingTo(enthalpy, pressureDrop);

    public new IFluidExtended HeatingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        (FluidExtended)base.HeatingTo(temperature, pressureDrop);

    public new IFluidExtended HeatingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null) =>
        (FluidExtended)base.HeatingTo(enthalpy, pressureDrop);

    public new IFluidExtended BubblePointAt(Pressure pressure) =>
        (FluidExtended)base.BubblePointAt(pressure);

    public new IFluidExtended BubblePointAt(Temperature temperature) =>
        (FluidExtended)base.BubblePointAt(temperature);

    public new IFluidExtended DewPointAt(Pressure pressure) =>
        (FluidExtended)base.DewPointAt(pressure);

    public new IFluidExtended DewPointAt(Temperature temperature) =>
        (FluidExtended)base.DewPointAt(temperature);

    public new IFluidExtended TwoPhasePointAt(Pressure pressure, Ratio quality) =>
        (FluidExtended)base.TwoPhasePointAt(pressure, quality);

    public IFluidExtended Mixing(
        Ratio firstSpecificMassFlow,
        IFluidExtended first,
        Ratio secondSpecificMassFlow,
        IFluidExtended second
    ) => (FluidExtended)base.Mixing(firstSpecificMassFlow, first, secondSpecificMassFlow, second);

    public new IFluidExtended Clone() => (FluidExtended)base.Clone();

    public new IFluidExtended Factory() => (FluidExtended)base.Factory();

    protected override AbstractFluid CreateInstance() =>
        new FluidExtended(Name, Fraction, CoolPropBackend);
}
