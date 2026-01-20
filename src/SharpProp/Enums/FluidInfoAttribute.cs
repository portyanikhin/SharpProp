namespace SharpProp;

/// <summary>
/// Fluid predefined info.
/// </summary>
/// <param name="name">CoolProp internal name.</param>
/// <param name="backend">Default type of CoolProp backend.</param>
/// <param name="pure">True if pure or pseudo-pure fluid.</param>
/// <param name="mixType">Mass-based or volume-based mixture.</param>
/// <param name="fractionMin">Minimum possible fraction.</param>
/// <param name="fractionMax">Maximum possible fraction.</param>
[AttributeUsage(AttributeTargets.Field)]
public sealed class FluidInfoAttribute(
    string name,
    string backend = "HEOS",
    bool pure = true,
    Mix mixType = Mix.Mass,
    double fractionMin = 0,
    double fractionMax = 1
) : Attribute
{
    /// <summary>
    /// CoolProp internal name.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Default type of CoolProp backend.
    /// </summary>
    public string Backend { get; } = backend;

    /// <summary>
    /// True if pure or pseudo-pure fluid.
    /// </summary>
    public bool Pure { get; } = pure;

    /// <summary>
    /// Mass-based or volume-based mixture.
    /// </summary>
    public Mix MixType { get; } = mixType;

    /// <summary>
    /// Minimum possible fraction.
    /// </summary>
    public double FractionMin { get; } = fractionMin;

    /// <summary>
    /// Maximum possible fraction.
    /// </summary>
    public double FractionMax { get; } = fractionMax;
}
