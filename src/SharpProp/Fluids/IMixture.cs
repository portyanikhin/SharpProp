namespace SharpProp;

/// <summary>
///     Mass-based mixture of pure fluids.
/// </summary>
public interface IMixture
    : IAbstractFluid,
        IClonable<IMixture>,
        IEquatable<IMixture>,
        IFactory<IMixture>,
        IJsonable
{
    /// <summary>
    ///     List of selected pure fluid names.
    /// </summary>
    IReadOnlyList<FluidsList> Fluids { get; }

    /// <summary>
    ///     List of mass-based fractions (by default, %).
    /// </summary>
    IReadOnlyList<Ratio> Fractions { get; }

    /// <summary>
    ///     Specify the phase state for all further calculations.
    /// </summary>
    /// <param name="phase">Phase state.</param>
    /// <returns>Current mixture instance.</returns>
    IMixture SpecifyPhase(Phases phase);

    /// <summary>
    ///     Unspecify the phase state and go back to calculating it based on the inputs.
    /// </summary>
    /// <returns>Current mixture instance.</returns>
    IMixture UnspecifyPhase();

    /// <summary>
    ///     Returns a new mixture instance with a defined state.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <returns>A new mixture instance with a defined state.</returns>
    /// <exception cref="ArgumentException">Need to define 2 unique inputs!</exception>
    IMixture WithState(IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput);

    /// <summary>
    ///     The process of cooling to given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the mixture at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process, the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IMixture CoolingTo(Temperature temperature, Pressure? pressureDrop = null);

    /// <summary>
    ///     The process of heating to given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the mixture at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     During the heating process, the temperature should increase!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IMixture HeatingTo(Temperature temperature, Pressure? pressureDrop = null);
}
