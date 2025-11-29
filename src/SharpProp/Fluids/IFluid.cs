namespace SharpProp;

/// <summary>
///     Pure/pseudo-pure fluid or binary mixture.
/// </summary>
public interface IFluid
    : IAbstractFluid,
        IClonable<IFluid>,
        IEquatable<IFluid>,
        IFactory<IFluid>,
        IJsonable
{
    /// <summary>
    ///     Selected fluid name.
    /// </summary>
    FluidsList Name { get; }

    /// <summary>
    ///     Mass-based or volume-based fraction
    ///     for binary mixtures (by default, %).
    /// </summary>
    Ratio Fraction { get; }

    /// <summary>
    ///    Type of CoolProp backend.
    /// </summary>
    string CoolPropBackend { get; }

    /// <summary>
    ///     Specify the phase state for all further calculations.
    /// </summary>
    /// <param name="phase">Phase state.</param>
    /// <returns>Current fluid instance.</returns>
    IFluid SpecifyPhase(Phases phase);

    /// <summary>
    ///     Unspecify the phase state and go back to calculating it based on the inputs.
    /// </summary>
    /// <returns>Current fluid instance.</returns>
    IFluid UnspecifyPhase();

    /// <summary>
    ///     Returns a new fluid instance with a defined state.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <returns>A new fluid instance with a defined state.</returns>
    /// <exception cref="ArgumentException">Need to define 2 unique inputs!</exception>
    IFluid WithState(IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput);

    /// <summary>
    ///     The process of isentropic compression to given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     Compressor outlet pressure should be higher than inlet pressure!
    /// </exception>
    IFluid IsentropicCompressionTo(Pressure pressure);

    /// <summary>
    ///     The process of compression to given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <param name="isentropicEfficiency">Compressor isentropic efficiency.</param>
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     Compressor outlet pressure should be higher than inlet pressure!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid compressor isentropic efficiency!</exception>
    IFluid CompressionTo(Pressure pressure, Ratio isentropicEfficiency);

    /// <summary>
    ///     The process of isenthalpic expansion to given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     Expansion valve outlet pressure should be lower than inlet pressure!
    /// </exception>
    IFluid IsenthalpicExpansionTo(Pressure pressure);

    /// <summary>
    ///     The process of isentropic expansion to given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     Expander outlet pressure should be lower than inlet pressure!
    /// </exception>
    IFluid IsentropicExpansionTo(Pressure pressure);

    /// <summary>
    ///     The process of expansion to given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <param name="isentropicEfficiency">Expander isentropic efficiency.</param>
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     Expander outlet pressure should be lower than inlet pressure!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid expander isentropic efficiency!</exception>
    IFluid ExpansionTo(Pressure pressure, Ratio isentropicEfficiency);

    /// <summary>
    ///     The process of cooling to given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process, the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IFluid CoolingTo(Temperature temperature, Pressure? pressureDrop = null);

    /// <summary>
    ///     The process of cooling to given enthalpy.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process, the enthalpy should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IFluid CoolingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null);

    /// <summary>
    ///     The process of heating to given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     During the heating process, the temperature should increase!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IFluid HeatingTo(Temperature temperature, Pressure? pressureDrop = null);

    /// <summary>
    ///     The process of heating to given enthalpy.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     During the heating process, the enthalpy should increase!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IFluid HeatingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null);

    /// <summary>
    ///     Returns a bubble point at given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>A bubble point at given pressure.</returns>
    IFluid BubblePointAt(Pressure pressure);

    /// <summary>
    ///     Returns a bubble point at given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <returns>A bubble point at given temperature.</returns>
    IFluid BubblePointAt(Temperature temperature);

    /// <summary>
    ///     Returns a dew point at given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>A dew point at given pressure.</returns>
    IFluid DewPointAt(Pressure pressure);

    /// <summary>
    ///     Returns a dew point at given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <returns>A dew point at given temperature.</returns>
    IFluid DewPointAt(Temperature temperature);

    /// <summary>
    ///     Returns a two-phase point at given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <param name="quality">Vapor quality.</param>
    /// <returns>Two-phase point at given pressure.</returns>
    IFluid TwoPhasePointAt(Pressure pressure, Ratio quality);

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
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     The mixing process is possible only for the same fluids!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     The mixing process is possible only for flows with the same pressure!
    /// </exception>
    IFluid Mixing(
        Ratio firstSpecificMassFlow,
        IFluid first,
        Ratio secondSpecificMassFlow,
        IFluid second
    );
}
