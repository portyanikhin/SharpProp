namespace SharpProp;

/// <summary>
///     Base class of fluids.
/// </summary>
public abstract partial class AbstractFluid : IEquatable<AbstractFluid>, IDisposable
{
    public void Dispose()
    {
        Backend.Dispose();
        GC.SuppressFinalize(this);
    }

    public bool Equals(AbstractFluid? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetHashCode() == other.GetHashCode();
    }

    /// <summary>
    ///     Returns a new fluid instance with no defined state.
    /// </summary>
    /// <returns>A new fluid instance with no defined state.</returns>
    public abstract AbstractFluid Factory();

    /// <summary>
    ///     Returns a new fluid instance with a defined state.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <returns>A new fluid object with a defined state.</returns>
    /// <exception cref="ArgumentException">Need to define 2 unique inputs!</exception>
    public virtual AbstractFluid WithState(IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput)
    {
        var fluid = Factory();
        fluid.Update(firstInput, secondInput);
        return fluid;
    }

    /// <summary>
    ///     Updates the state of the fluid.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <exception cref="ArgumentException">Need to define 2 unique inputs!</exception>
    public void Update(IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput)
    {
        Reset();
        var (inputPair, firstValue, secondValue) =
            GenerateUpdatePair(firstInput, secondInput);
        Backend.Update(inputPair, firstValue, secondValue);
        Inputs = new List<IKeyedInput<Parameters>> {firstInput, secondInput};
    }

    /// <summary>
    ///     Resets all non-trivial properties.
    /// </summary>
    protected virtual void Reset()
    {
        _compressibility = null;
        _conductivity = null;
        _density = null;
        _dynamicViscosity = null;
        _enthalpy = null;
        _entropy = null;
        _internalEnergy = null;
        _prandtl = null;
        _pressure = null;
        _quality = null;
        _soundSpeed = null;
        _specificHeat = null;
        _surfaceTension = null;
        _temperature = null;
        _phase = null;
    }

    /// <summary>
    ///     Returns true if the output is not null.
    /// </summary>
    /// <param name="key">The output key.</param>
    /// <param name="value">The output value.</param>
    /// <returns>True if the output is not null.</returns>
    protected bool KeyedOutputIsNotNull(Parameters key, out double? value)
    {
        value = NullableKeyedOutput(key);
        return value is not null;
    }

    /// <summary>
    ///     Returns a nullable keyed output.
    /// </summary>
    /// <param name="key">The output key.</param>
    /// <returns>A nullable keyed output.</returns>
    protected double? NullableKeyedOutput(Parameters key)
    {
        try
        {
            var value = KeyedOutput(key);
            return key is Parameters.iQ && value is < 0 or > 1 ? null : value;
        }
        catch (Exception e) when (e is ApplicationException or ArgumentException)
        {
            return null;
        }
    }

    /// <summary>
    ///     Returns a not nullable keyed output.
    /// </summary>
    /// <param name="key">The output key.</param>
    /// <returns>A not nullable keyed output.</returns>
    /// <exception cref="ArgumentException">Invalid or not defined state!</exception>
    protected double KeyedOutput(Parameters key)
    {
        var input = Inputs.Find(input => input.CoolPropKey == key)?.Value;
        var result = input ?? Backend.KeyedOutput(key);
        new OutputsValidator(result).Validate();
        return result;
    }

    private static (InputPairs inputPair, double firstValue, double secondValue) GenerateUpdatePair(
        IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput)
    {
        var inputPair = GetInputPair(firstInput, secondInput);
        var swap = !inputPair.HasValue;
        if (swap) inputPair = GetInputPair(secondInput, firstInput);
        if (!inputPair.HasValue)
            throw new ArgumentException("Need to define 2 unique inputs!");

        return !swap
            ? ((InputPairs) inputPair, firstValue: firstInput.Value, secondValue: secondInput.Value)
            : ((InputPairs) inputPair, firstValue: secondInput.Value, secondValue: firstInput.Value);
    }

    private static InputPairs? GetInputPair(
        IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput) =>
        AbstractState.GetInputPair($"{firstInput.CoolPropHighLevelKey}{secondInput.CoolPropHighLevelKey}_INPUTS");

    public override bool Equals(object? obj) => Equals(obj as AbstractFluid);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Compressibility);
        hashCode.Add(Conductivity);
        hashCode.Add(CriticalPressure);
        hashCode.Add(CriticalTemperature);
        hashCode.Add(Density);
        hashCode.Add(DynamicViscosity);
        hashCode.Add(Enthalpy);
        hashCode.Add(Entropy);
        hashCode.Add(FreezingTemperature);
        hashCode.Add(InternalEnergy);
        hashCode.Add(MaxPressure);
        hashCode.Add(MaxTemperature);
        hashCode.Add(MinPressure);
        hashCode.Add(MinTemperature);
        hashCode.Add(MolarMass);
        hashCode.Add(Phase);
        hashCode.Add(Prandtl);
        hashCode.Add(Pressure);
        hashCode.Add(Quality);
        hashCode.Add(SoundSpeed);
        hashCode.Add(SpecificHeat);
        hashCode.Add(SurfaceTension);
        hashCode.Add(Temperature);
        hashCode.Add(TriplePressure);
        hashCode.Add(TripleTemperature);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(AbstractFluid? left, AbstractFluid? right) => Equals(left, right);

    public static bool operator !=(AbstractFluid? left, AbstractFluid? right) => !Equals(left, right);
}