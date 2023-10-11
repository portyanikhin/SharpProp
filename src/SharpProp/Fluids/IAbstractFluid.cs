namespace SharpProp;

/// <summary>
///     Abstract fluid.
/// </summary>
public interface IAbstractFluid : IFluidState, IDisposable
{
    /// <summary>
    ///     Updates the state of the fluid.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <exception cref="ArgumentException">
    ///     Need to define 2 unique inputs!
    /// </exception>
    public void Update(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    );

    /// <summary>
    ///     Resets all non-trivial properties.
    /// </summary>
    public void Reset();

    /// <summary>
    ///     Specify the phase state for all further calculations.
    /// </summary>
    /// <param name="phase">Phase state.</param>
    public void SpecifyPhase(Phases phase);

    /// <summary>
    ///     Unspecify the phase state and
    ///     go back to calculating it based on the inputs.
    /// </summary>
    public void UnspecifyPhase();
}