using EnumsNET;
using SharpProp.Attributes;
using UnitsNet;
using UnitsNet.Units;

namespace SharpProp
{
    public static class FluidsListExtensions
    {
        /// <summary>
        ///     Gets access to the CoolProp internal name.
        /// </summary>
        /// <param name="member">The <see cref="FluidsList" /> member.</param>
        /// <returns>The CoolProp internal name.</returns>
        public static string CoolPropName(this FluidsList member) =>
            member.GetAttributes()!.Get<FluidInfo>()!.Name;

        /// <summary>
        ///     Gets access to the CoolProp backend type.
        /// </summary>
        /// <param name="member">The <see cref="FluidsList" /> member.</param>
        /// <returns>The CoolProp backend type.</returns>
        public static string CoolPropBackend(this FluidsList member) =>
            member.GetAttributes()!.Get<FluidInfo>()!.Backend;

        /// <summary>
        ///     Gets access to the type of the fluid (true if pure of pseudo-pure).
        /// </summary>
        /// <param name="member">The <see cref="FluidsList" /> member.</param>
        /// <returns>The type of the fluid.</returns>
        public static bool Pure(this FluidsList member) =>
            member.GetAttributes()!.Get<FluidInfo>()!.Pure;

        /// <summary>
        ///     Gets access to the mixture type.
        /// </summary>
        /// <param name="member">The <see cref="FluidsList" /> member.</param>
        /// <returns>The mixture type.</returns>
        public static Mix MixType(this FluidsList member) =>
            member.GetAttributes()!.Get<FluidInfo>()!.MixType;

        /// <summary>
        ///     Gets access to the minimum possible fraction.
        /// </summary>
        /// <param name="member">The <see cref="FluidsList" /> member.</param>
        /// <returns>The minimum possible fraction (by default, %).</returns>
        public static Ratio FractionMin(this FluidsList member) =>
            Ratio.FromDecimalFractions(member.GetAttributes()!.Get<FluidInfo>()!.FractionMin).ToUnit(RatioUnit.Percent);

        /// <summary>
        ///     Gets access to the maximum possible fraction.
        /// </summary>
        /// <param name="member">The <see cref="FluidsList" /> member.</param>
        /// <returns>The maximum possible fraction (by default, %).</returns>
        public static Ratio FractionMax(this FluidsList member) =>
            Ratio.FromDecimalFractions(member.GetAttributes()!.Get<FluidInfo>()!.FractionMax).ToUnit(RatioUnit.Percent);
    }
}