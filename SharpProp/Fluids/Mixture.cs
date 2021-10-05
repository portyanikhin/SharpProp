using System;
using System.Collections.Generic;
using System.Linq;
using CoolProp;
using SharpProp.Extensions;

namespace SharpProp
{
    /// <summary>
    ///     CoolProp mass-based mixture of pure fluids
    /// </summary>
    public class Mixture : AbstractFluid, IEquatable<Mixture>
    {
        /// <summary>
        ///     CoolProp mass-based mixture of pure fluids
        /// </summary>
        /// <param name="fluids">List of selected pure fluids</param>
        /// <param name="fractions">List of mass-based fractions (with values from 0 to 1)</param>
        /// <exception cref="ArgumentException">
        ///     Invalid input! Fluids and Fractions should be of the same length.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Invalid components! All of them should be a pure fluid with HEOS backend.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Invalid component mass fractions! All of them should be in (0;1).
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Invalid component mass fractions! Their sum should be equal to 1.
        /// </exception>
        public Mixture(List<FluidsList> fluids, List<double> fractions)
        {
            if (fluids.Count != fractions.Count)
                throw new ArgumentException("Invalid input! Fluids and Fractions should be of the same length.");
            if (!fluids.All(fluid => fluid.Pure() && fluid.CoolPropBackend() == "HEOS"))
                throw new ArgumentException(
                    "Invalid components! All of them should be a pure fluid with HEOS backend.");
            if (!fractions.All(frac => frac is > 0 and < 1))
                throw new ArgumentException("Invalid component mass fractions! All of them should be in (0;1).");
            if (Math.Abs(fractions.Sum(frac => frac) - 1) > 1e-6)
                throw new ArgumentException("Invalid component mass fractions! Their sum should be equal to 1.");
            Fluids = fluids;
            Fractions = fractions;
            Backend = AbstractState.factory("HEOS", string.Join("&", Fluids.ToArray()));
            Backend.set_mass_fractions(new DoubleVector(Fractions));
        }

        /// <summary>
        ///     List of selected pure fluids
        /// </summary>
        public List<FluidsList> Fluids { get; }

        /// <summary>
        ///     List of mass-based fractions (with values from 0 to 1)
        /// </summary>
        public List<double> Fractions { get; }

        public bool Equals(Mixture? other) =>
            base.Equals(other) && Fluids.SequenceEqual(other.Fluids) && Fractions.SequenceEqual(other.Fractions);

        public override Mixture Factory() => new(Fluids, Fractions);

        public override Mixture WithState(IKeyedInput<parameters> firstInput, IKeyedInput<parameters> secondInput) =>
            (Mixture) base.WithState(firstInput, secondInput);

        public new bool Equals(object? obj) => Equals(obj as Mixture);

        public override int GetHashCode() =>
            HashCode.Combine(string.Join("&", Fluids), string.Join("&", Fractions), base.GetHashCode());
    }
}