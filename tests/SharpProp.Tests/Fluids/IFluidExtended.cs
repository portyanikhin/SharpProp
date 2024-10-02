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
    SpecificEntropy SpecificHeatConstVolume { get; }

    /// <summary>
    ///     Molar density (by default, kg/mol).
    /// </summary>
    MolarMass? MolarDensity { get; }

    /// <summary>
    ///     Ozone depletion potential (ODP).
    /// </summary>
    double? OzoneDepletionPotential { get; }

    /// <inheritdoc cref="IFluid.SpecifyPhase"/>
    new IFluidExtended SpecifyPhase(Phases phase);

    /// <inheritdoc cref="IFluid.UnspecifyPhase"/>
    new IFluidExtended UnspecifyPhase();

    /// <inheritdoc cref="IFluid.WithState"/>
    new IFluidExtended WithState(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    );

    /// <inheritdoc cref="IFluid.IsentropicCompressionTo"/>
    new IFluidExtended IsentropicCompressionTo(Pressure pressure);

    /// <inheritdoc cref="IFluid.CompressionTo"/>
    new IFluidExtended CompressionTo(Pressure pressure, Ratio isentropicEfficiency);

    /// <inheritdoc cref="IFluid.IsenthalpicExpansionTo"/>
    new IFluidExtended IsenthalpicExpansionTo(Pressure pressure);

    /// <inheritdoc cref="IFluid.IsentropicExpansionTo"/>
    new IFluidExtended IsentropicExpansionTo(Pressure pressure);

    /// <inheritdoc cref="IFluid.IsentropicExpansionTo"/>
    new IFluidExtended ExpansionTo(Pressure pressure, Ratio isentropicEfficiency);

    /// <inheritdoc cref="IFluid.CoolingTo(Temperature, Pressure?)"/>
    new IFluidExtended CoolingTo(Temperature temperature, Pressure? pressureDrop = null);

    /// <inheritdoc cref="IFluid.CoolingTo(SpecificEnergy, Pressure?)"/>
    new IFluidExtended CoolingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null);

    /// <inheritdoc cref="IFluid.HeatingTo(Temperature, Pressure?)"/>
    new IFluidExtended HeatingTo(Temperature temperature, Pressure? pressureDrop = null);

    /// <inheritdoc cref="IFluid.HeatingTo(SpecificEnergy, Pressure?)"/>
    new IFluidExtended HeatingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null);

    /// <inheritdoc cref="IFluid.BubblePointAt(Pressure)"/>
    new IFluidExtended BubblePointAt(Pressure pressure);

    /// <inheritdoc cref="IFluid.BubblePointAt(Temperature)"/>
    new IFluidExtended BubblePointAt(Temperature temperature);

    /// <inheritdoc cref="IFluid.DewPointAt(Pressure)"/>
    new IFluidExtended DewPointAt(Pressure pressure);

    /// <inheritdoc cref="IFluid.DewPointAt(Temperature)"/>
    new IFluidExtended DewPointAt(Temperature temperature);

    /// <inheritdoc cref="IFluid.TwoPhasePointAt"/>
    new IFluidExtended TwoPhasePointAt(Pressure pressure, Ratio quality);

    /// <inheritdoc cref="IFluid.Mixing"/>
    IFluidExtended Mixing(
        Ratio firstSpecificMassFlow,
        IFluidExtended first,
        Ratio secondSpecificMassFlow,
        IFluidExtended second
    );

    /// <inheritdoc cref="IClonable{T}.Clone"/>
    new IFluidExtended Clone();

    /// <inheritdoc cref="IFactory{T}.Factory"/>
    new IFluidExtended Factory();
}
