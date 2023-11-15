namespace SharpProp.Tests;

/// <summary>
///     An example of how to add new properties to
///     the <see cref="IFluid"/> interface.
/// </summary>
public interface IFluidExtended : IFluid
{
    /// <summary>
    ///     Mass specific constant volume specific heat (by default, kJ/kg/K).
    /// </summary>
    public SpecificEntropy SpecificHeatConstVolume { get; }

    /// <summary>
    ///     Molar density (by default, kg/mol).
    /// </summary>
    public MolarMass? MolarDensity { get; }

    /// <summary>
    ///     Ozone depletion potential (ODP).
    /// </summary>
    public double? OzoneDepletionPotential { get; }

    /// <inheritdoc cref="IFluid.WithState"/>
    public new IFluidExtended WithState(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    );

    /// <inheritdoc cref="IFluid.IsentropicCompressionTo"/>
    public new IFluidExtended IsentropicCompressionTo(Pressure pressure);

    /// <inheritdoc cref="IFluid.CompressionTo"/>
    public new IFluidExtended CompressionTo(
        Pressure pressure,
        Ratio isentropicEfficiency
    );

    /// <inheritdoc cref="IFluid.IsenthalpicExpansionTo"/>
    public new IFluidExtended IsenthalpicExpansionTo(Pressure pressure);

    /// <inheritdoc cref="IFluid.IsentropicExpansionTo"/>
    public new IFluidExtended IsentropicExpansionTo(Pressure pressure);

    /// <inheritdoc cref="IFluid.IsentropicExpansionTo"/>
    public new IFluidExtended ExpansionTo(
        Pressure pressure,
        Ratio isentropicEfficiency
    );

    /// <inheritdoc cref="IFluid.CoolingTo(Temperature, Pressure?)"/>
    public new IFluidExtended CoolingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    );

    /// <inheritdoc cref="IFluid.CoolingTo(SpecificEnergy, Pressure?)"/>
    public new IFluidExtended CoolingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    );

    /// <inheritdoc cref="IFluid.HeatingTo(Temperature, Pressure?)"/>
    public new IFluidExtended HeatingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    );

    /// <inheritdoc cref="IFluid.HeatingTo(SpecificEnergy, Pressure?)"/>
    public new IFluidExtended HeatingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    );

    /// <inheritdoc cref="IFluid.BubblePointAt(Pressure)"/>
    public new IFluidExtended BubblePointAt(Pressure pressure);

    /// <inheritdoc cref="IFluid.BubblePointAt(Temperature)"/>
    public new IFluidExtended BubblePointAt(Temperature temperature);

    /// <inheritdoc cref="IFluid.DewPointAt(Pressure)"/>
    public new IFluidExtended DewPointAt(Pressure pressure);

    /// <inheritdoc cref="IFluid.DewPointAt(Temperature)"/>
    public new IFluidExtended DewPointAt(Temperature temperature);

    /// <inheritdoc cref="IFluid.TwoPhasePointAt"/>
    public new IFluidExtended TwoPhasePointAt(Pressure pressure, Ratio quality);

    /// <inheritdoc cref="IFluid.Mixing"/>
    public IFluidExtended Mixing(
        Ratio firstSpecificMassFlow,
        IFluidExtended first,
        Ratio secondSpecificMassFlow,
        IFluidExtended second
    );

    /// <inheritdoc cref="IClonable{T}.Clone"/>
    public new IFluidExtended Clone();

    /// <inheritdoc cref="IFactory{T}.Factory"/>
    public new IFluidExtended Factory();
}
