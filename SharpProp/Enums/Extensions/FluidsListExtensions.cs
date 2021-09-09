﻿using SharpProp.Attributes;

namespace SharpProp.Extensions
{
    public static class FluidsListExtensions
    {
        /// <summary>
        ///     Gets access to the CoolProp internal name
        /// </summary>
        /// <param name="member">The <see cref="FluidsList" /> member</param>
        /// <returns>The CoolProp internal name</returns>
        public static string CoolPropName(this FluidsList member) => member.GetAttribute<FluidInfo>().Name;

        /// <summary>
        ///     Gets access to the CoolProp backend type
        /// </summary>
        /// <param name="member">The <see cref="FluidsList" /> member</param>
        /// <returns>The CoolProp backend type</returns>
        public static string CoolPropBackend(this FluidsList member) => member.GetAttribute<FluidInfo>().Backend;

        /// <summary>
        ///     Gets access to the type of the fluid (true if pure of pseudo-pure)
        /// </summary>
        /// <param name="member">The <see cref="FluidsList" /> member</param>
        /// <returns>The type of the fluid</returns>
        public static bool Pure(this FluidsList member) => member.GetAttribute<FluidInfo>().Pure;

        /// <summary>
        ///     Gets access to the mixture type
        /// </summary>
        /// <param name="member">The <see cref="FluidsList" /> member</param>
        /// <returns>The mixture type</returns>
        public static Mix MixType(this FluidsList member) => member.GetAttribute<FluidInfo>().MixType;

        /// <summary>
        ///     Gets access to the minimum possible fraction
        /// </summary>
        /// <param name="member">The <see cref="FluidsList" /> member</param>
        /// <returns>The minimum possible fraction</returns>
        public static double FractionMin(this FluidsList member) => member.GetAttribute<FluidInfo>().FractionMin;

        /// <summary>
        ///     Gets access to the maximum possible fraction
        /// </summary>
        /// <param name="member">The <see cref="FluidsList" /> member</param>
        /// <returns>The maximum possible fraction</returns>
        public static double FractionMax(this FluidsList member) => member.GetAttribute<FluidInfo>().FractionMax;
    }
}