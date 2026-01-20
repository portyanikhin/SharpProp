namespace SharpProp;

/// <summary>
/// CoolProp keyed input.
/// </summary>
/// <typeparam name="T">
/// Type of Coolprop internal keys (<see cref="Parameters"/> for fluids and mixtures;
/// <see cref="string"/> for humid air).
/// </typeparam>
public interface IKeyedInput<out T>
{
    /// <summary>
    /// CoolProp internal key.
    /// </summary>
    T CoolPropKey { get; }

    /// <summary>
    /// CoolProp key in high-level interface.
    /// </summary>
    string CoolPropHighLevelKey { get; }

    /// <summary>
    /// Input value in SI units.
    /// </summary>
    double Value { get; }
}

/// <inheritdoc cref="IKeyedInput{T}"/>
/// <param name="CoolPropKey">CoolProp internal key.</param>
/// <param name="Value">Input value in SI units.</param>
public abstract record KeyedInput<T>(T CoolPropKey, double Value) : IKeyedInput<T>
{
    public T CoolPropKey { get; } = CoolPropKey;
    public abstract string CoolPropHighLevelKey { get; }
    public double Value { get; } = Value;
}
