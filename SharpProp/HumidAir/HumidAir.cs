namespace SharpProp;

/// <summary>
///     Real humid air (see ASHRAE RP-1485).
/// </summary>
public partial class HumidAir : IEquatable<HumidAir>
{
    public bool Equals(HumidAir? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetHashCode() == other.GetHashCode();
    }

    /// <summary>
    ///     Returns a new humid air instance with no defined state.
    /// </summary>
    /// <returns>A new humid air instance with no defined state.</returns>
    public virtual HumidAir Factory() => new();

    /// <summary>
    ///     Returns a new humid air instance with a defined state.
    /// </summary>
    /// <param name="fistInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <param name="thirdInput">Third input property.</param>
    /// <returns>A new humid air instance with a defined state.</returns>
    /// <exception cref="ArgumentException">
    ///     Need to define 3 unique inputs!
    /// </exception>
    public virtual HumidAir WithState(IKeyedInput<string> fistInput,
        IKeyedInput<string> secondInput, IKeyedInput<string> thirdInput)
    {
        var humidAir = Factory();
        humidAir.Update(fistInput, secondInput, thirdInput);
        return humidAir;
    }

    /// <summary>
    ///     Updates the state of the humid air.
    /// </summary>
    /// <param name="firstInput">First input property.</param>
    /// <param name="secondInput">Second input property.</param>
    /// <param name="thirdInput">Third input property.</param>
    /// <exception cref="ArgumentException">Need to define 3 unique inputs!</exception>
    public void Update(IKeyedInput<string> firstInput,
        IKeyedInput<string> secondInput, IKeyedInput<string> thirdInput)
    {
        Reset();
        Inputs = new List<IKeyedInput<string>> {firstInput, secondInput, thirdInput};
        CheckInputs();
    }

    /// <summary>
    ///     Resets all properties.
    /// </summary>
    protected virtual void Reset()
    {
        Inputs.Clear();
        _compressibility = null;
        _conductivity = null;
        _density = null;
        _dewTemperature = null;
        _dynamicViscosity = null;
        _enthalpy = null;
        _entropy = null;
        _humidity = null;
        _partialPressure = null;
        _pressure = null;
        _relativeHumidity = null;
        _specificHeat = null;
        _temperature = null;
        _wetBulbTemperature = null;
    }

    /// <summary>
    ///     Returns a not nullable keyed output.
    /// </summary>
    /// <param name="key">The output key.</param>
    /// <returns>A not nullable keyed output.</returns>
    /// <exception cref="ArgumentException">Need to define 3 unique inputs!</exception>
    /// <exception cref="ArgumentException">Invalid or not defined state!</exception>
    protected double KeyedOutput(string key)
    {
        CheckInputs();
        var input = Inputs.Find(input => input.CoolPropKey == key)?.Value;
        var result = input ?? CoolProp.HAPropsSI(key, Inputs[0].CoolPropKey, Inputs[0].Value,
            Inputs[1].CoolPropKey, Inputs[1].Value, Inputs[2].CoolPropKey, Inputs[2].Value);
        new OutputsValidator(result).Validate();
        return result;
    }

    private void CheckInputs()
    {
        var uniqueKeys = Inputs.Select(input => input.CoolPropKey).Distinct().ToList();
        if (Inputs.Count != 3 || uniqueKeys.Count != 3)
            throw new ArgumentException("Need to define 3 unique inputs!");
    }

    public override bool Equals(object? obj) => Equals(obj as HumidAir);

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode() =>
        HashCode.Combine(
            string.Join("&", Inputs.Select(input => input.Value)),
            string.Join("&", Inputs.Select(input => input.CoolPropKey)));

    public static bool operator ==(HumidAir? left, HumidAir? right) => Equals(left, right);

    public static bool operator !=(HumidAir? left, HumidAir? right) => !Equals(left, right);
}