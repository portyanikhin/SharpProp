namespace SharpProp;

/// <summary>
/// Extension methods for the <see cref="FluidsList"/>.
/// </summary>
public static class FluidsListExtensions
{
    /// <summary>
    /// CoolProp internal name.
    /// </summary>
    /// <param name="member">The <see cref="FluidsList"/> member.</param>
    /// <returns>CoolProp internal name.</returns>
    public static string CoolPropName(this FluidsList member) =>
        member.GetAttributes()!.Get<FluidInfoAttribute>()!.Name;

    /// <summary>
    /// Default type of CoolProp backend.
    /// </summary>
    /// <param name="member">The <see cref="FluidsList"/> member.</param>
    /// <returns>Default type of CoolProp backend.</returns>
    public static string CoolPropBackend(this FluidsList member) =>
        member.GetAttributes()!.Get<FluidInfoAttribute>()!.Backend;

    /// <summary>
    /// <c>true</c> if the fluid is pure of pseudo-pure.
    /// </summary>
    /// <param name="member">The <see cref="FluidsList"/> member.</param>
    /// <returns><c>true</c> if the fluid is pure of pseudo-pure.</returns>
    public static bool Pure(this FluidsList member) =>
        member.GetAttributes()!.Get<FluidInfoAttribute>()!.Pure;

    /// <summary>
    /// Mixture type.
    /// </summary>
    /// <param name="member">The <see cref="FluidsList"/> member.</param>
    /// <returns>Mixture type.</returns>
    public static Mix MixType(this FluidsList member) =>
        member.GetAttributes()!.Get<FluidInfoAttribute>()!.MixType;

    /// <summary>
    /// Minimum possible fraction.
    /// </summary>
    /// <param name="member">The <see cref="FluidsList"/> member.</param>
    /// <returns>Minimum possible fraction (by default, %).</returns>
    public static Ratio FractionMin(this FluidsList member) =>
        Ratio
            .FromDecimalFractions(member.GetAttributes()!.Get<FluidInfoAttribute>()!.FractionMin)
            .ToUnit(RatioUnit.Percent);

    /// <summary>
    /// Maximum possible fraction.
    /// </summary>
    /// <param name="member">The <see cref="FluidsList"/> member.</param>
    /// <returns>Maximum possible fraction (by default, %).</returns>
    public static Ratio FractionMax(this FluidsList member) =>
        Ratio
            .FromDecimalFractions(member.GetAttributes()!.Get<FluidInfoAttribute>()!.FractionMax)
            .ToUnit(RatioUnit.Percent);
}
