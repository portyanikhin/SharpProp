using UnitsNet.NumberExtensions.NumberToRatio;

namespace SharpProp;

/// <summary>
///     Pure/pseudo-pure fluid or binary mixture.
/// </summary>
public class Fluid :
    AbstractFluid,
    IClonable<Fluid>,
    IEquatable<Fluid>,
    IFactory<Fluid>,
    IJsonable
{
    /// <summary>
    ///     Pure/pseudo-pure fluid or binary mixture.
    /// </summary>
    /// <param name="name">Selected fluid name.</param>
    /// <param name="fraction">
    ///     Mass-based or volume-based fraction
    ///     for binary mixtures (optional).
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
        if (fraction is not null
            && (fraction < name.FractionMin() ||
                fraction > name.FractionMax()))
            throw new ArgumentException(
                "Invalid fraction value! " +
                $"It should be in [{name.FractionMin().Percent};" +
                $"{name.FractionMax().Percent}] %. " +
                $"Entered value = {fraction.Value.Percent} %."
            );
        Name = name;
        Fraction = Name.Pure()
            ? Ratio.FromPercent(100)
            : fraction?.ToUnit(RatioUnit.Percent) ??
              throw new ArgumentException(
                  "Need to define the fraction!"
              );
        Backend = AbstractState.Factory(
            Name.CoolPropBackend(),
            Name.CoolPropName()
        );
        if (!Name.Pure()) SetFraction();
    }

    /// <summary>
    ///     Selected fluid name.
    /// </summary>
    public FluidsList Name { get; }

    /// <summary>
    ///     Mass-based or volume-based fraction
    ///     for binary mixtures (by default, %).
    /// </summary>
    public Ratio Fraction { get; }

    public Fluid Clone() =>
        WithState(Inputs[0], Inputs[1]);

    public bool Equals(Fluid? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetHashCode() == other.GetHashCode();
    }

    public Fluid Factory() =>
        (Fluid) CreateInstance();

    public string AsJson(bool indented = true) =>
        this.ConvertToJson(indented);

    public override bool Equals(object? obj) =>
        Equals(obj as Fluid);

    public override int GetHashCode() =>
        (Name.CoolPropName(),
            Fraction.DecimalFractions,
            base.GetHashCode())
        .GetHashCode();

    public static bool operator ==(Fluid? left, Fluid? right) =>
        Equals(left, right);

    public static bool operator !=(Fluid? left, Fluid? right) =>
        !Equals(left, right);

    /// <summary>
    ///     Returns a new fluid
    ///     instance with a defined state.
    /// </summary>
    /// <param name="firstInput">
    ///     First input property.
    /// </param>
    /// <param name="secondInput">
    ///     Second input property.
    /// </param>
    /// <returns>
    ///     A new fluid instance
    ///     with a defined state.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Need to define 2 unique inputs!
    /// </exception>
    public new Fluid WithState(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    ) => (Fluid) base.WithState(firstInput, secondInput);

    /// <summary>
    ///     The process of isentropic
    ///     compression to a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>
    ///     The state of the fluid
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Compressor outlet pressure
    ///     should be higher than inlet pressure!
    /// </exception>
    public new Fluid IsentropicCompressionTo(Pressure pressure) =>
        (Fluid) base.IsentropicCompressionTo(pressure);

    /// <summary>
    ///     The process of compression
    ///     to a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <param name="isentropicEfficiency">
    ///     Compressor isentropic efficiency.
    /// </param>
    /// <returns>
    ///     The state of the fluid
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Compressor outlet pressure
    ///     should be higher than inlet pressure!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid compressor isentropic efficiency!
    /// </exception>
    public new Fluid CompressionTo(
        Pressure pressure,
        Ratio isentropicEfficiency
    ) => (Fluid) base.CompressionTo(
        pressure,
        isentropicEfficiency
    );

    /// <summary>
    ///     The process of isenthalpic
    ///     expansion to a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>
    ///     The state of the fluid
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Expansion valve outlet pressure
    ///     should be lower than inlet pressure!
    /// </exception>
    public new Fluid IsenthalpicExpansionTo(Pressure pressure) =>
        (Fluid) base.IsenthalpicExpansionTo(pressure);

    /// <summary>
    ///     The process of isentropic
    ///     expansion to a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>
    ///     The state of the fluid
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Expander outlet pressure
    ///     should be lower than inlet pressure!
    /// </exception>
    public new Fluid IsentropicExpansionTo(Pressure pressure) =>
        (Fluid) base.IsentropicExpansionTo(pressure);

    /// <summary>
    ///     The process of expansion
    ///     to a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <param name="isentropicEfficiency">
    ///     Expander isentropic efficiency.
    /// </param>
    /// <returns>
    ///     The state of the fluid
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Expander outlet pressure
    ///     should be lower than inlet pressure!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid expander isentropic efficiency!
    /// </exception>
    public new Fluid ExpansionTo(
        Pressure pressure,
        Ratio isentropicEfficiency
    ) => (Fluid) base.ExpansionTo(
        pressure,
        isentropicEfficiency
    );

    /// <summary>
    ///     The process of cooling
    ///     to a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in
    ///     the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the fluid
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process,
    ///     the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop
    ///     in the heat exchanger!
    /// </exception>
    public new Fluid CoolingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    ) => (Fluid) base.CoolingTo(temperature, pressureDrop);

    /// <summary>
    ///     The process of cooling
    ///     to a given enthalpy.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in
    ///     the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the fluid
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process,
    ///     the enthalpy should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop
    ///     in the heat exchanger!
    /// </exception>
    public new Fluid CoolingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    ) => (Fluid) base.CoolingTo(enthalpy, pressureDrop);

    /// <summary>
    ///     The process of heating
    ///     to a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in the heat exchanger (optional)
    /// </param>
    /// <returns>
    ///     The state of the fluid
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the heating process,
    ///     the temperature should increase!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop
    ///     in the heat exchanger!
    /// </exception>
    public new Fluid HeatingTo(
        Temperature temperature,
        Pressure? pressureDrop = null
    ) => (Fluid) base.HeatingTo(temperature, pressureDrop);

    /// <summary>
    ///     The process of heating
    ///     to a given enthalpy.
    /// </summary>
    /// <param name="enthalpy">Enthalpy.</param>
    /// <param name="pressureDrop">
    ///     Pressure drop in
    ///     the heat exchanger (optional).
    /// </param>
    /// <returns>
    ///     The state of the fluid
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     During the heating process,
    ///     the enthalpy should increase!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Invalid pressure drop
    ///     in the heat exchanger!
    /// </exception>
    public new Fluid HeatingTo(
        SpecificEnergy enthalpy,
        Pressure? pressureDrop = null
    ) => (Fluid) base.HeatingTo(enthalpy, pressureDrop);

    /// <summary>
    ///     Returns a bubble point
    ///     at a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>
    ///     A bubble point at a given pressure.
    /// </returns>
    public new Fluid BubblePointAt(Pressure pressure) =>
        (Fluid) base.BubblePointAt(pressure);

    /// <summary>
    ///     Returns a bubble point
    ///     at a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <returns>
    ///     A bubble point at a given temperature.
    /// </returns>
    public new Fluid BubblePointAt(Temperature temperature) =>
        (Fluid) base.BubblePointAt(temperature);

    /// <summary>
    ///     Returns a dew point
    ///     at a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <returns>
    ///     A dew point at a given pressure.
    /// </returns>
    public new Fluid DewPointAt(Pressure pressure) =>
        (Fluid) base.DewPointAt(pressure);

    /// <summary>
    ///     Returns a dew point
    ///     at a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <returns>
    ///     A dew point at a given temperature.
    /// </returns>
    public new Fluid DewPointAt(Temperature temperature) =>
        (Fluid) base.DewPointAt(temperature);

    /// <summary>
    ///     Returns a two-phase point
    ///     at a given pressure.
    /// </summary>
    /// <param name="pressure">Pressure.</param>
    /// <param name="quality">Vapor quality.</param>
    /// <returns>
    ///     Two-phase point at a given pressure.
    /// </returns>
    public new Fluid TwoPhasePointAt(
        Pressure pressure,
        Ratio quality
    ) => (Fluid) base.TwoPhasePointAt(pressure, quality);

    /// <summary>
    ///     The mixing process.
    /// </summary>
    /// <param name="firstSpecificMassFlow">
    ///     Specific mass flow rate
    ///     of the fluid at the first state.
    /// </param>
    /// <param name="first">
    ///     Fluid at the first state.
    /// </param>
    /// <param name="secondSpecificMassFlow">
    ///     Specific mass flow rate
    ///     of the fluid at the second state.
    /// </param>
    /// <param name="second">
    ///     Fluid at the second state.
    /// </param>
    /// <returns>
    ///     The state of the fluid
    ///     at the end of the process.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     The mixing process is possible
    ///     only for the same fluids!
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     The mixing process is possible
    ///     only for flows with the same pressure!
    /// </exception>
    public Fluid Mixing(
        Ratio firstSpecificMassFlow,
        Fluid first,
        Ratio secondSpecificMassFlow,
        Fluid second
    ) => IsValidFluidsForMixing(first, second)
        ? (Fluid) base.Mixing(
            firstSpecificMassFlow,
            first,
            secondSpecificMassFlow,
            second
        )
        : throw new ArgumentException(
            "The mixing process is possible " +
            "only for the same fluids!"
        );

    protected override AbstractFluid CreateInstance() =>
        new Fluid(Name, Fraction);

    private bool IsValidFluidsForMixing(
        Fluid first,
        Fluid second
    ) => Name == first.Name &&
         first.Name == second.Name &&
         Fraction.Equals(
             first.Fraction,
             Tolerance.DecimalFractions()
         ) && first.Fraction.Equals(
             second.Fraction,
             Tolerance.DecimalFractions()
         );

    private void SetFraction()
    {
        var fractionsVector = new DoubleVector(
            new[] {Fraction.DecimalFractions}
        );
        if (Name.MixType() is Mix.Mass)
            Backend.SetMassFractions(fractionsVector);
        else
            Backend.SetVolumeFractions(fractionsVector);
    }
}