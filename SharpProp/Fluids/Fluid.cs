using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp;

/// <summary>
///     Pure/pseudo-pure fluid or binary mixture.
/// </summary>
public class Fluid : AbstractFluid, IFluid
{
    /// <summary>
    ///     Pure/pseudo-pure fluid or binary mixture.
    /// </summary>
    /// <param name="name">Selected fluid name.</param>
    /// <param name="fraction">
    ///     Mass-based or volume-based fraction for binary mixtures (optional).
    /// </param>
    /// <exception cref="ArgumentException">
    ///     Invalid fraction value! It should be in
    ///     [{fractionMin};{fractionMax}] %.
    ///     Entered value = {fraction} %.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Need to define the fraction!
    /// </exception>
    public Fluid(FluidsList name, Ratio? fraction = null)
    {
        if (
            fraction is not null
            && (fraction < name.FractionMin() || fraction > name.FractionMax())
        )
            throw new ArgumentException(
                "Invalid fraction value! "
                    + $"It should be in [{name.FractionMin().Percent};"
                    + $"{name.FractionMax().Percent}] %. "
                    + $"Entered value = {fraction.Value.Percent} %."
            );
        Name = name;
        Fraction = Name.Pure()
            ? Ratio.FromPercent(100)
            : fraction?.ToUnit(RatioUnit.Percent)
                ?? throw new ArgumentException("Need to define the fraction!");
        Backend = AbstractState.Factory(
            Name.CoolPropBackend(),
            Name.CoolPropName()
        );
        if (!Name.Pure())
            SetFraction();
    }

    public FluidsList Name { get; }

    public Ratio Fraction { get; }

    public new IFluid WithState(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    ) => (Fluid)base.WithState(firstInput, secondInput);

    public new IFluid IsentropicCompressionTo(Pressure pressure) =>
        (Fluid)base.IsentropicCompressionTo(pressure);

    public new IFluid CompressionTo(
        Pressure pressure,
        Ratio isentropicEfficiency
    ) => (Fluid)base.CompressionTo(pressure, isentropicEfficiency);

    public new IFluid IsenthalpicExpansionTo(Pressure pressure) =>
        (Fluid)base.IsenthalpicExpansionTo(pressure);

    public new IFluid IsentropicExpansionTo(Pressure pressure) =>
        (Fluid)base.IsentropicExpansionTo(pressure);

    public new IFluid ExpansionTo(
        Pressure pressure,
        Ratio isentropicEfficiency
    ) => (Fluid)base.ExpansionTo(pressure, isentropicEfficiency);

    public new IFluid CoolingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    ) => (Fluid)base.CoolingTo(temperature, pressureDrop);

    public new IFluid CoolingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    ) => (Fluid)base.CoolingTo(enthalpy, pressureDrop);

    public new IFluid HeatingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    ) => (Fluid)base.HeatingTo(temperature, pressureDrop);

    public new IFluid HeatingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    ) => (Fluid)base.HeatingTo(enthalpy, pressureDrop);

    public new IFluid BubblePointAt(Pressure pressure) =>
        (Fluid)base.BubblePointAt(pressure);

    public new IFluid BubblePointAt(Temperature temperature) =>
        (Fluid)base.BubblePointAt(temperature);

    public new IFluid DewPointAt(Pressure pressure) =>
        (Fluid)base.DewPointAt(pressure);

    public new IFluid DewPointAt(Temperature temperature) =>
        (Fluid)base.DewPointAt(temperature);

    public new IFluid TwoPhasePointAt(Pressure pressure, Ratio quality) =>
        (Fluid)base.TwoPhasePointAt(pressure, quality);

    public IFluid Mixing(
        Ratio firstSpecificMassFlow,
        IFluid first,
        Ratio secondSpecificMassFlow,
        IFluid second
    ) =>
        IsValidFluidsForMixing(first, second)
            ? (Fluid)
                base.Mixing(
                    firstSpecificMassFlow,
                    first,
                    secondSpecificMassFlow,
                    second
                )
            : throw new ArgumentException(
                "The mixing process is possible only for the same fluids!"
            );

    public IFluid Clone() => WithState(Inputs[0], Inputs[1]);

    public IFluid Factory() => (Fluid)CreateInstance();

    public string AsJson(bool indented = true) => this.ConvertToJson(indented);

    public bool Equals(IFluid? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return GetHashCode() == other.GetHashCode();
    }

    public override bool Equals(object? obj) => Equals(obj as Fluid);

    public override int GetHashCode() =>
        (
            Name.CoolPropName(),
            Fraction.DecimalFractions,
            base.GetHashCode()
        ).GetHashCode();

    protected override AbstractFluid CreateInstance() =>
        new Fluid(Name, Fraction);

    private bool IsValidFluidsForMixing(IFluid first, IFluid second) =>
        Name == first.Name
        && first.Name == second.Name
        && Fraction.Equals(first.Fraction, Tolerance.DecimalFractions())
        && first.Fraction.Equals(second.Fraction, Tolerance.DecimalFractions());

    private void SetFraction()
    {
        var fractionsVector = new DoubleVector(
            new[] { Fraction.DecimalFractions }
        );
        if (Name.MixType() is Mix.Mass)
            Backend.SetMassFractions(fractionsVector);
        else
            Backend.SetVolumeFractions(fractionsVector);
    }
}
