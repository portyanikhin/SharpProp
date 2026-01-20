namespace SharpProp;

/// <summary>
/// Abstract fluid.
/// </summary>
public interface IAbstractFluid : IFluidState, IDisposable
{
    /// <summary>
    /// Updates the state of the fluid.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <exception cref="ArgumentException">Need to define 2 unique inputs!</exception>
    void Update(IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput);

    /// <summary>
    /// Resets all non-trivial properties.
    /// </summary>
    void Reset();
}
