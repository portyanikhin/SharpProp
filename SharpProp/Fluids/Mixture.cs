using System;
using System.Collections.Generic;
using System.Linq;
using CoolProp;
using SharpProp.Extensions;
using UnitsNet;
using UnitsNet.Units;

namespace SharpProp
{
    /// <summary>
    ///     CoolProp mass-based mixture of pure fluids.
    /// </summary>
#pragma warning disable CA1067
    public class Mixture : AbstractFluid, IEquatable<Mixture>
#pragma warning restore CA1067
    {
        /// <summary>
        ///     CoolProp mass-based mixture of pure fluids.
        /// </summary>
        /// <param name="fluids">List of selected pure fluids.</param>
        /// <param name="fractions">List of mass-based fractions.</param>
        /// <exception cref="ArgumentException">
        ///     Invalid input! Fluids and Fractions should be of the same length.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Invalid components! All of them should be a pure fluid with HEOS backend.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Invalid component mass fractions! All of them should be in (0;100) %.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Invalid component mass fractions! Their sum should be equal to 100 %.
        /// </exception>
        public Mixture(List<FluidsList> fluids, IReadOnlyCollection<Ratio> fractions)
        {
            if (fluids.Count != fractions.Count)
                throw new ArgumentException("Invalid input! Fluids and Fractions should be of the same length.");
            if (!fluids.All(fluid => fluid.Pure() && fluid.CoolPropBackend() == "HEOS"))
                throw new ArgumentException(
                    "Invalid components! All of them should be a pure fluid with HEOS backend.");
            if (!fractions.All(frac => frac.Percent is > 0 and < 100))
                throw new ArgumentException("Invalid component mass fractions! All of them should be in (0;100) %.");
            if (Math.Abs(fractions.Sum(frac => frac.Percent) - 100) > 1e-6)
                throw new ArgumentException("Invalid component mass fractions! Their sum should be equal to 100 %.");
            Fluids = fluids;
            Fractions = fractions.Select(frac => frac.ToUnit(RatioUnit.Percent)).ToList();
            Backend = AbstractState.factory("HEOS", string.Join("&", Fluids.ToArray()));
            Backend.set_mass_fractions(new DoubleVector(Fractions.Select(frac => frac.DecimalFractions)));
        }

        /// <summary>
        ///     List of selected pure fluids.
        /// </summary>
        public List<FluidsList> Fluids { get; }

        /// <summary>
        ///     List of mass-based fractions (by default, %).
        /// </summary>
        public List<Ratio> Fractions { get; }

        public bool Equals(Mixture? other) => base.Equals(other);

        public override Mixture Factory() => new(Fluids, Fractions);

        public override Mixture WithState(IKeyedInput<Parameters> firstInput,
            IKeyedInput<Parameters> secondInput) =>
            (Mixture) base.WithState(firstInput, secondInput);

        public new bool Equals(object? obj) => Equals(obj as Mixture);

        public override int GetHashCode() =>
            HashCode.Combine(string.Join("&", Fluids),
                string.Join("&", Fractions), base.GetHashCode());
    }
}