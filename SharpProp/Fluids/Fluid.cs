using System;
using CoolProp;
using SharpProp.Extensions;

namespace SharpProp
{
    public class Fluid : AbstractFluid
    {
        /// <summary>
        ///     CoolProp pure/pseudo-pure fluid or binary mixture
        /// </summary>
        /// <param name="name">Selected fluid</param>
        /// <param name="fraction">Mass-based or volume-based fraction for binary mixtures (optional)</param>
        /// <exception cref="ArgumentException">
        ///     Invalid fraction value! It should be in [{fractionMin};{fractionMax}]. Entered value = {fraction}
        /// </exception>
        /// <exception cref="ArgumentException">Need to define fraction!</exception>
        public Fluid(FluidsList name, double? fraction = null)
        {
            if (fraction is not null && (fraction < name.FractionMin() || fraction > name.FractionMax()))
                throw new ArgumentException(
                    $"Invalid fraction value! It should be in [{name.FractionMin()};{name.FractionMax()}]. " +
                    $"Entered value = {fraction}");
            Name = name;
            Fraction = Name.Pure() ? 1 : fraction ?? throw new ArgumentException("Need to define fraction!");
            Backend = AbstractState.factory(Name.CoolPropBackend(), Name.CoolPropName());
            if (!Name.Pure()) SetFraction();
        }

        /// <summary>
        ///     Selected fluid
        /// </summary>
        public FluidsList Name { get; }

        /// <summary>
        ///     Mass-based or volume-based fraction for binary mixtures (from 0 to 1)
        /// </summary>
        public double Fraction { get; }

        public override Fluid Factory() => new(Name, Fraction);

        public override Fluid WithState(IKeyedInput<parameters> firstInput, IKeyedInput<parameters> secondInput) =>
            (Fluid) base.WithState(firstInput, secondInput);

        private void SetFraction()
        {
            var fractionsVector = new DoubleVector(new[] {Fraction});
            if (Name.MixType() is Mix.Mass)
                Backend.set_mass_fractions(fractionsVector);
            else
                Backend.set_volu_fractions(fractionsVector);
        }
    }
}