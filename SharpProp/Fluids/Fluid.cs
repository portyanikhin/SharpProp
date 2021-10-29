using System;
using CoolProp;
using SharpProp.Extensions;
using UnitsNet;
using UnitsNet.Units;

namespace SharpProp
{
    /// <summary>
    ///     CoolProp pure/pseudo-pure fluid or binary mixture.
    /// </summary>
#pragma warning disable CA1067
    public class Fluid : AbstractFluid, IEquatable<Fluid>
#pragma warning restore CA1067
    {
        /// <summary>
        ///     CoolProp pure/pseudo-pure fluid or binary mixture.
        /// </summary>
        /// <param name="name">Selected fluid.</param>
        /// <param name="fraction">Mass-based or volume-based fraction for binary mixtures (optional).</param>
        /// <exception cref="ArgumentException">
        ///     Invalid fraction value! It should be in [{fractionMin};{fractionMax}] %. Entered value = {fraction} %.
        /// </exception>
        /// <exception cref="ArgumentException">Need to define fraction!</exception>
        public Fluid(FluidsList name, Ratio? fraction = null)
        {
            if (fraction is not null && (fraction < name.FractionMin() || fraction > name.FractionMax()))
                throw new ArgumentException(
                    "Invalid fraction value! " +
                    $"It should be in [{name.FractionMin().Percent};{name.FractionMax().Percent}] %. " +
                    $"Entered value = {fraction.Value.Percent} %.");
            Name = name;
            Fraction = Name.Pure()
                ? Ratio.FromPercent(100)
                : fraction?.ToUnit(RatioUnit.Percent) ?? throw new ArgumentException("Need to define fraction!");
            Backend = AbstractState.factory(Name.CoolPropBackend(), Name.CoolPropName());
            if (!Name.Pure()) SetFraction();
        }

        /// <summary>
        ///     Selected fluid.
        /// </summary>
        public FluidsList Name { get; }

        /// <summary>
        ///     Mass-based or volume-based fraction for binary mixtures (by default, %).
        /// </summary>
        public Ratio Fraction { get; }

        public override Fluid Factory() => new(Name, Fraction);

        public override Fluid WithState(IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput) =>
            (Fluid) base.WithState(firstInput, secondInput);

        private void SetFraction()
        {
            var fractionsVector = new DoubleVector(new[] {Fraction.DecimalFractions});
            if (Name.MixType() is Mix.Mass)
                Backend.set_mass_fractions(fractionsVector);
            else
                Backend.set_volu_fractions(fractionsVector);
        }

        public bool Equals(Fluid? other) => base.Equals(other) && (Name, Fraction) == (other.Name, other.Fraction);
        
        public new bool Equals(object? obj) => Equals(obj as Fluid);

        public override int GetHashCode() => HashCode.Combine(Name, Fraction, base.GetHashCode());
    }
}