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

    /// <summary>
    ///     Returns a new fluid instance with a defined state.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <returns>
    ///     A new fluid instance with a defined state.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Need to define 2 unique inputs!
    /// </exception>
    public new IFluidExtended WithState(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    );

    /// <summary>
    ///     The process of isentropic compression to a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>
    ///     The state of the fluid at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Compressor outlet pressure should be higher than inlet pressure!
    /// </exception>
    public new IFluidExtended IsentropicCompressionTo(Pressure pressure);

    /// <summary>
    ///     The process of compression to a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <param name="isentropicEfficiency">
    ///     Compressor isentropic efficiency.
    /// </param>
    /// <returns>
    ///     The state of the fluid at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Compressor outlet pressure should be higher than inlet pressure!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid compressor isentropic efficiency!
    /// </exception>
    public new IFluidExtended CompressionTo(
        Pressure pressure,
        Ratio isentropicEfficiency
    );

    /// <summary>
    ///     The process of isenthalpic expansion to a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>
    ///     The state of the fluid at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Expansion valve outlet pressure should be lower than inlet pressure!
    /// </exception>
    public new IFluidExtended IsenthalpicExpansionTo(Pressure pressure);

    /// <summary>
    ///     The process of isentropic expansion to a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>
    ///     The state of the fluid at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Expander outlet pressure should be lower than inlet pressure!
    /// </exception>
    public new IFluidExtended IsentropicExpansionTo(Pressure pressure);

    /// <summary>
    ///     The process of expansion to a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <param name="isentropicEfficiency">
    ///     Expander isentropic efficiency.
    /// </param>
    /// <returns>
    ///     The state of the fluid at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Expander outlet pressure should be lower than inlet pressure!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid expander isentropic efficiency!
    /// </exception>
    public new IFluidExtended ExpansionTo(
        Pressure pressure,
        Ratio isentropicEfficiency
    );

    /// <summary>
    ///     The process of cooling to a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the fluid at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process, the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop in the heat exchanger!
    /// </exception>
    public new IFluidExtended CoolingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    );

    /// <summary>
    ///     The process of cooling to a given enthalpy.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the fluid at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process, the enthalpy should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop in the heat exchanger!
    /// </exception>
    public new IFluidExtended CoolingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    );

    /// <summary>
    ///     The process of heating to a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the fluid at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the heating process, the temperature should increase!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop in the heat exchanger!
    /// </exception>
    public new IFluidExtended HeatingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    );

    /// <summary>
    ///     The process of heating to a given enthalpy.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the fluid at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the heating process, the enthalpy should increase!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop in the heat exchanger!
    /// </exception>
    public new IFluidExtended HeatingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    );

    /// <summary>
    ///     Returns a bubble point at a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>
    ///     A bubble point at a given pressure.
    /// </returns>
    public new IFluidExtended BubblePointAt(Pressure pressure);

    /// <summary>
    ///     Returns a bubble point at a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <returns>
    ///     A bubble point at a given temperature.
    /// </returns>
    public new IFluidExtended BubblePointAt(Temperature temperature);

    /// <summary>
    ///     Returns a dew point at a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>
    ///     A dew point at a given pressure.
    /// </returns>
    public new IFluidExtended DewPointAt(Pressure pressure);

    /// <summary>
    ///     Returns a dew point at a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <returns>
    ///     A dew point at a given temperature.
    /// </returns>
    public new IFluidExtended DewPointAt(Temperature temperature);

    /// <summary>
    ///     Returns a two-phase point at a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <param name="quality">Vapor quality.</param>
    /// <returns>
    ///     Two-phase point at a given pressure.
    /// </returns>
    public new IFluidExtended TwoPhasePointAt(Pressure pressure, Ratio quality);

    /// <summary>
    ///     The mixing process.
    /// </summary>
    /// <param name="firstSpecificMassFlow">
    ///     Specific mass flow rate of the fluid at the first state.
    /// </param>
    /// <param name="first">Fluid at the first state.</param>
    /// <param name="secondSpecificMassFlow">
    ///     Specific mass flow rate of the fluid at the second state.
    /// </param>
    /// <param name="second">Fluid at the second state.</param>
    /// <returns>
    ///     The state of the fluid at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     The mixing process is possible only for the same fluids!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     The mixing process is possible only for flows with the same pressure!
    /// </exception>
    public IFluidExtended Mixing(
        Ratio firstSpecificMassFlow,
        IFluidExtended first,
        Ratio secondSpecificMassFlow,
        IFluidExtended second
    );

    /// <summary>
    ///     Performs deep (full) copy of the instance.
    /// </summary>
    /// <returns>Deep copy of the instance.</returns>
    public new IFluidExtended Clone();

    /// <summary>
    ///     Creates a new instance with no defined state.
    /// </summary>
    /// <returns>
    ///     A new instance with no defined state.
    /// </returns>
    public new IFluidExtended Factory();
}
