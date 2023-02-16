namespace SharpProp;

/// <summary>
///     Mass-based mixture of pure fluids.
/// </summary>
public class Mixture : AbstractFluid, IClonable<Mixture>, IEquatable<Mixture>, IFactory<Mixture>, IJsonable
{
    private const string AvailableBackend = "HEOS";

    /// <summary>
    ///     Mass-based mixture of pure fluids.
    /// </summary>
    /// <param name="fluids">List of selected names of pure fluids.</param>
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
    public Mixture(IEnumerable<FluidsList> fluids, IEnumerable<Ratio> fractions)
    {
        Fluids = fluids.ToList();
        Fractions = fractions.Select(
            fraction => fraction.ToUnit(RatioUnit.Percent)).ToList();
        if (Fluids.Count != Fractions.Count)
            throw new ArgumentException(
                "Invalid input! Fluids and Fractions should be of the same length.");
        if (!Fluids.All(fluid => fluid.Pure() && fluid.CoolPropBackend() == AvailableBackend))
            throw new ArgumentException(
                $"Invalid components! All of them should be a pure fluid with {AvailableBackend} backend.");
        if (!Fractions.All(fraction => fraction.Percent is > 0 and < 100))
            throw new ArgumentException(
                "Invalid component mass fractions! All of them should be in (0;100) %.");
        if (Math.Abs(Fractions.Sum(fraction => fraction.Percent) - 100) > 1e-6)
            throw new ArgumentException(
                "Invalid component mass fractions! Their sum should be equal to 100 %.");
        Backend = AbstractState.Factory(
            AvailableBackend, string.Join("&", Fluids.Select(fluid => fluid.CoolPropName())));
        Backend.SetMassFractions(
            new DoubleVector(Fractions.Select(fraction => fraction.DecimalFractions)));
    }

    /// <summary>
    ///     List of selected names of pure fluids.
    /// </summary>
    public List<FluidsList> Fluids { get; }

    /// <summary>
    ///     List of mass-based fractions (by default, %).
    /// </summary>
    public List<Ratio> Fractions { get; }

    public Mixture Clone() => WithState(Inputs[0], Inputs[1]);

    public bool Equals(Mixture? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetHashCode() == other.GetHashCode();
    }

    public Mixture Factory() => (Mixture) CreateInstance();

    public string AsJson(bool indented = true) => this.ConvertToJson(indented);

    public override bool Equals(object? obj) => Equals(obj as Mixture);

    public override int GetHashCode() =>
        (string.Join("&", Fluids.Select(fluid => fluid.CoolPropName())),
            string.Join("&", Fractions.Select(fraction => fraction.DecimalFractions)),
            base.GetHashCode())
        .GetHashCode();

    public static bool operator ==(Mixture? left, Mixture? right) => Equals(left, right);

    public static bool operator !=(Mixture? left, Mixture? right) => !Equals(left, right);

    /// <summary>
    ///     Returns a new mixture instance with a defined state.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <returns>A new mixture instance with a defined state.</returns>
    /// <exception cref="ArgumentException">Need to define 2 unique inputs!</exception>
    public new Mixture WithState(IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput) =>
        (Mixture) base.WithState(firstInput, secondInput);

    /// <summary>
    ///     The process of cooling to a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     During the cooling process, the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    public new Mixture CoolingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        (Mixture) base.CoolingTo(temperature, pressureDrop);

    /// <summary>
    ///     The process of heating to a given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the fluid at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    ///     During the heating process, the temperature should increase!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    public new Mixture HeatingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        (Mixture) base.HeatingTo(temperature, pressureDrop);

    protected override AbstractFluid CreateInstance() => new Mixture(Fluids, Fractions);
}