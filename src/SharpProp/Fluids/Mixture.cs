namespace SharpProp;

/// <summary>
/// Mass-based mixture of pure fluids.
/// </summary>
public interface IMixture
    : IAbstractFluid,
        IClonable<IMixture>,
        IEquatable<IMixture>,
        IFactory<IMixture>,
        IJsonable
{
    /// <summary>
    /// List of selected pure fluid names.
    /// </summary>
    IReadOnlyList<FluidsList> Fluids { get; }

    /// <summary>
    /// List of mass-based fractions (by default, %).
    /// </summary>
    IReadOnlyList<Ratio> Fractions { get; }

    /// <summary>
    /// Specify the phase state for all further calculations.
    /// </summary>
    /// <param name="phase">Phase state.</param>
    /// <returns>Current mixture instance.</returns>
    IMixture SpecifyPhase(Phases phase);

    /// <summary>
    /// Unspecify the phase state and go back to calculating it based on the inputs.
    /// </summary>
    /// <returns>Current mixture instance.</returns>
    IMixture UnspecifyPhase();

    /// <summary>
    /// Returns a new mixture instance with a defined state.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <returns>A new mixture instance with a defined state.</returns>
    /// <exception cref="ArgumentException">Need to define 2 unique inputs!</exception>
    IMixture WithState(IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput);

    /// <summary>
    /// The process of cooling to given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the mixture at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the cooling process, the temperature should decrease!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IMixture CoolingTo(Temperature temperature, Pressure? pressureDrop = null);

    /// <summary>
    /// The process of heating to given temperature.
    /// </summary>
    /// <param name="temperature">Temperature.</param>
    /// <param name="pressureDrop">Pressure drop in the heat exchanger (optional).</param>
    /// <returns>The state of the mixture at the end of the process.</returns>
    /// <exception cref="ArgumentException">
    /// During the heating process, the temperature should increase!
    /// </exception>
    /// <exception cref="ArgumentException">Invalid pressure drop in the heat exchanger!</exception>
    IMixture HeatingTo(Temperature temperature, Pressure? pressureDrop = null);
}

/// <inheritdoc cref="IMixture"/>
public class Mixture : AbstractFluid, IMixture
{
    private const string AvailableBackend = "HEOS";

    /// <inheritdoc cref="Mixture"/>
    /// <param name="fluids">List of selected pure fluid names.</param>
    /// <param name="fractions">List of mass-based fractions.</param>
    /// <exception cref="ArgumentException">
    /// Invalid input! Fluids and Fractions should be of the same length.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Invalid components! All of them should be a pure fluid with HEOS backend.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Invalid component mass fractions! All of them should be in (0;100) %.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Invalid component mass fractions! Their sum should be equal to 100 %.
    /// </exception>
    public Mixture(IEnumerable<FluidsList> fluids, IEnumerable<Ratio> fractions)
    {
        Fluids = [.. fluids];
        Fractions = [.. fractions.Select(fraction => fraction.ToUnit(RatioUnit.Percent))];
        if (Fluids.Count != Fractions.Count)
        {
            throw new ArgumentException(
                "Invalid input! Fluids and Fractions should be of the same length."
            );
        }

        if (!Fluids.All(fluid => fluid.Pure() && fluid.CoolPropBackend() == AvailableBackend))
        {
            throw new ArgumentException(
                "Invalid components! All of them should be a pure fluid with "
                    + $"{AvailableBackend} backend."
            );
        }

        if (!Fractions.All(fraction => fraction.Percent is > 0 and < 100))
        {
            throw new ArgumentException(
                "Invalid component mass fractions! All of them should be in (0;100) %."
            );
        }

        if (Math.Abs(Fractions.Sum(fraction => fraction.Percent) - 100) > 1e-6)
        {
            throw new ArgumentException(
                "Invalid component mass fractions! Their sum should be equal to 100 %."
            );
        }

        Backend = AbstractState.Factory(
            AvailableBackend,
            string.Join("&", Fluids.Select(fluid => fluid.CoolPropName()))
        );
        Backend.SetMassFractions(
            new DoubleVector(Fractions.Select(fraction => fraction.DecimalFractions))
        );
    }

    public IReadOnlyList<FluidsList> Fluids { get; }

    public IReadOnlyList<Ratio> Fractions { get; }

    public new IMixture SpecifyPhase(Phases phase) => (Mixture)base.SpecifyPhase(phase);

    public new IMixture UnspecifyPhase() => (Mixture)base.UnspecifyPhase();

    public new IMixture WithState(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    ) => (Mixture)base.WithState(firstInput, secondInput);

    public new IMixture CoolingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        (Mixture)base.CoolingTo(temperature, pressureDrop);

    public new IMixture HeatingTo(Temperature temperature, Pressure? pressureDrop = null) =>
        (Mixture)base.HeatingTo(temperature, pressureDrop);

    public IMixture Clone() => WithState(Inputs[0], Inputs[1]);

    public IMixture Factory() => (Mixture)CreateInstance();

    public string AsJson(bool indented = true) => this.ConvertToJson(indented);

    public bool Equals(IMixture? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || GetHashCode() == other.GetHashCode();
    }

    public override bool Equals(object? obj) => Equals(obj as Mixture);

    public override int GetHashCode() =>
        (
            string.Join("&", Fluids.Select(fluid => fluid.CoolPropName())),
            string.Join("&", Fractions.Select(fraction => fraction.DecimalFractions)),
            base.GetHashCode()
        ).GetHashCode();

    protected override AbstractFluid CreateInstance() => new Mixture(Fluids, Fractions);
}
