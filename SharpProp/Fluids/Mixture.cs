namespace SharpProp;

/// <summary>
///     Mass-based mixture of pure fluids.
/// </summary>
#pragma warning disable CA1067
public class Mixture : AbstractFluid, IEquatable<Mixture>
#pragma warning restore CA1067
{
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
        if (!Fluids.All(fluid => fluid.Pure() && fluid.CoolPropBackend() == "HEOS"))
            throw new ArgumentException(
                "Invalid components! All of them should be a pure fluid with HEOS backend.");
        if (!Fractions.All(fraction => fraction.Percent is > 0 and < 100))
            throw new ArgumentException(
                "Invalid component mass fractions! All of them should be in (0;100) %.");
        if (Math.Abs(Fractions.Sum(fraction => fraction.Percent) - 100) > 1e-6)
            throw new ArgumentException(
                "Invalid component mass fractions! Their sum should be equal to 100 %.");
        Backend = AbstractState.Factory("HEOS", string.Join("&", Fluids.ToArray()));
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

    public bool Equals(Mixture? other) => base.Equals(other);

    public override Mixture Factory() => new(Fluids, Fractions);

    public override Mixture WithState(IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput) =>
        (Mixture) base.WithState(firstInput, secondInput);

    public override Mixture CoolingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        (Mixture) base.CoolingTo(temperature, pressureDrop);

    public override Mixture HeatingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        (Mixture) base.HeatingTo(temperature, pressureDrop);

    public new bool Equals(object? obj) => Equals(obj as Mixture);

    public override int GetHashCode() =>
        HashCode.Combine(string.Join("&", Fluids),
            string.Join("&", Fractions), base.GetHashCode());
}