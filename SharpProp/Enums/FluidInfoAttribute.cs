﻿namespace SharpProp;

/// <summary>
///     Fluid predefined info.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class FluidInfoAttribute : Attribute
{
    /// <summary>
    ///     Fluid predefined info.
    /// </summary>
    /// <param name="name">CoolProp internal name.</param>
    /// <param name="backend">Type of CoolProp backend.</param>
    /// <param name="pure">True if pure or pseudo-pure fluid.</param>
    /// <param name="mixType">Mass-based or volume-based mixture.</param>
    /// <param name="fractionMin">Minimum possible fraction.</param>
    /// <param name="fractionMax">Maximum possible fraction.</param>
    public FluidInfoAttribute(
        string name,
        string backend = "HEOS",
        bool pure = true,
        Mix mixType = Mix.Mass,
        double fractionMin = 0,
        double fractionMax = 1
    )
    {
        Name = name;
        Backend = backend;
        Pure = pure;
        MixType = mixType;
        FractionMin = fractionMin;
        FractionMax = fractionMax;
    }

    /// <summary>
    ///     CoolProp internal name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Type of CoolProp backend.
    /// </summary>
    public string Backend { get; }

    /// <summary>
    ///     True if pure or pseudo-pure fluid.
    /// </summary>
    public bool Pure { get; }

    /// <summary>
    ///     Mass-based or volume-based mixture.
    /// </summary>
    public Mix MixType { get; }

    /// <summary>
    ///     Minimum possible fraction.
    /// </summary>
    public double FractionMin { get; }

    /// <summary>
    ///     Maximum possible fraction.
    /// </summary>
    public double FractionMax { get; }
}
