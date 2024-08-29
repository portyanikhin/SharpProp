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
    ///     Returns a new mixture instance with a defined state.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <returns>A new mixture instance with a defined state.</returns>
    /// <exception cref="ArgumentException">
    ///     Need to define 2 unique inputs!
    /// </exception>
    IMixture WithState(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    );

    /// <summary>
    ///     The process of cooling to a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in the heat exchanger (optional).
    /// </param>
    /// <returns>The state of the mixture at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process, the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop in the heat exchanger!
    /// </exception>
    IMixture CoolingTo(Temperature temperature, Pressure? pressureDrop = null);

    /// <summary>
    ///     The process of heating to a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in the heat exchanger (optional).
    /// </param>
    /// <returns>The state of the mixture at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     During the heating process, the temperature should increase!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop in the heat exchanger!
    /// </exception>
    IMixture HeatingTo(Temperature temperature, Pressure? pressureDrop = null);
}
