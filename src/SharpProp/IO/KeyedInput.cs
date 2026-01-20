namespace SharpProp;

/// <inheritdoc cref="IKeyedInput{T}"/>
/// <param name="CoolPropKey">CoolProp internal key.</param>
/// <param name="Value">Input value in SI units.</param>
public abstract record KeyedInput<T>(T CoolPropKey, double Value) : IKeyedInput<T>
{
    public T CoolPropKey { get; } = CoolPropKey;
    public abstract string CoolPropHighLevelKey { get; }
    public double Value { get; } = Value;
}
