namespace SharpProp;

/// <summary>
///     CoolProp keyed input.
/// </summary>
/// <typeparam name="T">
///     Type of Coolprop internal keys
///     (<see cref="Parameters"/> for fluids and mixtures;
///     <see cref="string"/> for humid air).
/// </typeparam>
public interface IKeyedInput<out T>
{
    /// <summary>
    ///     CoolProp internal key.
    /// </summary>
    T CoolPropKey { get; }

    /// <summary>
    ///     CoolProp key in high-level interface.
    /// </summary>
    string CoolPropHighLevelKey { get; }

    /// <summary>
    ///     Input value in SI units.
    /// </summary>
    double Value { get; }
}
