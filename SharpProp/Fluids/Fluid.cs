using System;
using UnitsNet;
using UnitsNet.Units;

namespace SharpProp;

/// <summary>
///     Pure/pseudo-pure fluid or binary mixture.
/// </summary>
#pragma warning disable CA1067
public class Fluid : AbstractFluid, IEquatable<Fluid>
#pragma warning restore CA1067
{
    /// <summary>
    ///     Pure/pseudo-pure fluid or binary mixture.
    /// </summary>
    /// <param name="name">Selected fluid name.</param>
    /// <param name="fraction">Mass-based or volume-based fraction for binary mixtures (optional).</param>
    /// <exception cref="ArgumentException">
    ///     Invalid fraction value! It should be in [{fractionMin};{fractionMax}] %. Entered value = {fraction} %.
    /// </exception>
    /// <exception cref="ArgumentException">Need to define the fraction!</exception>
    public Fluid(FluidsList name, Ratio? fraction = null)
    {
        if (fraction is not null && (fraction < name.FractionMin() || fraction > name.FractionMax()))
            throw new ArgumentException(
                "Invalid fraction value! " +
                $"It should be in [{name.FractionMin().Percent};" +
                $"{name.FractionMax().Percent}] %. " +
                $"Entered value = {fraction.Value.Percent} %.");
        Name = name;
        Fraction = Name.Pure()
            ? Ratio.FromPercent(100)
            : fraction?.ToUnit(RatioUnit.Percent) ??
              throw new ArgumentException("Need to define the fraction!");
        Backend = AbstractState.Factory(Name.CoolPropBackend(), Name.CoolPropName());
        if (!Name.Pure()) SetFraction();
    }

    /// <summary>
    ///     Selected fluid name.
    /// </summary>
    public FluidsList Name { get; }

    /// <summary>
    ///     Mass-based or volume-based fraction for binary mixtures (by default, %).
    /// </summary>
    public Ratio Fraction { get; }

    public bool Equals(Fluid? other) => base.Equals(other);

    public override Fluid Factory() => new(Name, Fraction);

    public override Fluid WithState(IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput) =>
        (Fluid) base.WithState(firstInput, secondInput);

    public override Fluid IsentropicCompressionTo(Pressure pressure) =>
        (Fluid) base.IsentropicCompressionTo(pressure);

    public override Fluid CompressionTo(Pressure pressure, Ratio isentropicEfficiency) =>
        (Fluid) base.CompressionTo(pressure, isentropicEfficiency);

    public override Fluid IsenthalpicExpansionTo(Pressure pressure) =>
        (Fluid) base.IsenthalpicExpansionTo(pressure);

    public override Fluid IsentropicExpansionTo(Pressure pressure) =>
        (Fluid) base.IsentropicExpansionTo(pressure);

    public override Fluid ExpansionTo(Pressure pressure, Ratio isentropicEfficiency) =>
        (Fluid) base.ExpansionTo(pressure, isentropicEfficiency);

    public override Fluid CoolingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        (Fluid) base.CoolingTo(temperature, pressureDrop);

    public override Fluid CoolingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null) =>
        (Fluid) base.CoolingTo(enthalpy, pressureDrop);

    public override Fluid HeatingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        (Fluid) base.HeatingTo(temperature, pressureDrop);

    public override Fluid HeatingTo(SpecificEnergy enthalpy, Pressure? pressureDrop = null) =>
        (Fluid) base.HeatingTo(enthalpy, pressureDrop);

    public override Fluid BubblePointAt(Pressure pressure) =>
        (Fluid) base.BubblePointAt(pressure);

    public override Fluid BubblePointAt(Temperature temperature) =>
        (Fluid) base.BubblePointAt(temperature);

    public override Fluid DewPointAt(Pressure pressure) =>
        (Fluid) base.DewPointAt(pressure);

    public override Fluid DewPointAt(Temperature temperature) =>
        (Fluid) base.DewPointAt(temperature);

    public override Fluid TwoPhasePointAt(Pressure pressure, Ratio quality) =>
        (Fluid) base.TwoPhasePointAt(pressure, quality);

    /// <summary>
    ///     The mixing process.
    /// </summary>
    /// <param name="firstSpecificMassFlow">
    ///     Specific mass flow rate of the fluid at the first state.
    /// </param>
    /// <param name="first">Fluid at the first state.</param>
    /// <param name="secondSpecificMassFlow">
    ///     Specific mass flow rate of the fluid at the second state.
    /// </param>
    /// <param name="second">Fluid at the second state.</param>
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">The mixing process is possible only for the same fluids!</exception>
    /// <exception cref="ArgumentException">
    ///     The mixing process is possible only for flows with the same pressure!
    /// </exception>
    public override Fluid Mixing(Ratio firstSpecificMassFlow, AbstractFluid first,
        Ratio secondSpecificMassFlow, AbstractFluid second) =>
        IsValidFluidsForMixing(first, second)
            ? (Fluid) base.Mixing(firstSpecificMassFlow, first, secondSpecificMassFlow, second)
            : throw new ArgumentException(
                "The mixing process is possible only for the same fluids!");

    private bool IsValidFluidsForMixing(AbstractFluid first, AbstractFluid second) =>
        first is Fluid firstFluid &&
        second is Fluid secondFluid &&
        Name == firstFluid.Name &&
        firstFluid.Name == secondFluid.Name &&
        Fraction == firstFluid.Fraction &&
        firstFluid.Fraction == secondFluid.Fraction;

    private void SetFraction()
    {
        var fractionsVector = new DoubleVector(new[] {Fraction.DecimalFractions});
        if (Name.MixType() is Mix.Mass)
            Backend.SetMassFractions(fractionsVector);
        else
            Backend.SetVolumeFractions(fractionsVector);
    }

    public new bool Equals(object? obj) => Equals(obj as Fluid);

    public override int GetHashCode() =>
        HashCode.Combine(Name, Fraction, base.GetHashCode());
}