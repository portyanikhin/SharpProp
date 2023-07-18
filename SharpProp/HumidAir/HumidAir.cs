namespace SharpProp;

/// <summary>
///     Real humid air (see ASHRAE RP-1485).
/// </summary>
public partial class HumidAir :
    IClonable<HumidAir>,
    IEquatable<HumidAir>,
    IFactory<HumidAir>,
    IJsonable
{
    private List<IKeyedInput<string>> _inputs = new(3);

    public HumidAir Clone() =>
        WithState(_inputs[0], _inputs[1], _inputs[2]);

    public bool Equals(HumidAir? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetHashCode() == other.GetHashCode();
    }

    public HumidAir Factory() =>
        CreateInstance();

    public string AsJson(bool indented = true) =>
        this.ConvertToJson(indented);

    public override bool Equals(object? obj) =>
        Equals(obj as HumidAir);

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode() =>
        (string.Join("&", _inputs.Select(input => input.Value)),
            string.Join("&", _inputs.Select(input => input.CoolPropKey)))
        .GetHashCode();

    public static bool operator ==(HumidAir? left, HumidAir? right) =>
        Equals(left, right);

    public static bool operator !=(HumidAir? left, HumidAir? right) =>
        !Equals(left, right);

    /// <summary>
    ///     Returns a new humid air
    ///     instance with a defined state.
    /// </summary>
    /// <param name="fistInput">
    ///     First input property.
    /// </param>
    /// <param name="secondInput">
    ///     Second input property.
    /// </param>
    /// <param name="thirdInput">
    ///     Third input property.
    /// </param>
    /// <returns>
    ///     A new humid air instance
    ///     with a defined state.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Need to define 3 unique inputs!
    /// </exception>
    public HumidAir WithState(
        IKeyedInput<string> fistInput,
        IKeyedInput<string> secondInput,
        IKeyedInput<string> thirdInput
    )
    {
        var humidAir = CreateInstance();
        humidAir.Update(fistInput, secondInput, thirdInput);
        return humidAir;
    }

    /// <summary>
    ///     Updates the state of the humid air.
    /// </summary>
    /// <param name="firstInput">
    ///     First input property.
    /// </param>
    /// <param name="secondInput">
    ///     Second input property.
    /// </param>
    /// <param name="thirdInput">
    ///     Third input property.
    /// </param>
    /// <exception cref="ArgumentException">
    ///     Need to define 3 unique inputs!
    /// </exception>
    public void Update(
        IKeyedInput<string> firstInput,
        IKeyedInput<string> secondInput,
        IKeyedInput<string> thirdInput
    )
    {
        Reset();
        _inputs = new List<IKeyedInput<string>>
            {firstInput, secondInput, thirdInput};
        CheckInputs();
    }

    protected virtual void Reset()
    {
        _inputs.Clear();
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

    protected virtual HumidAir CreateInstance() => new();

    protected double KeyedOutput(string key)
    {
        CheckInputs();
        var input = _inputs
            .Find(input => input.CoolPropKey == key)?
            .Value;
        var result = input ?? CoolProp.HAPropsSI(
            key,
            _inputs[0].CoolPropKey,
            _inputs[0].Value,
            _inputs[1].CoolPropKey,
            _inputs[1].Value,
            _inputs[2].CoolPropKey,
            _inputs[2].Value
        );
        new OutputsValidator(result).Validate();
        return result;
    }

    private void CheckInputs()
    {
        var uniqueKeys = _inputs
            .Select(input => input.CoolPropKey)
            .Distinct()
            .ToList();
        if (_inputs.Count != 3 ||
            uniqueKeys.Count != 3)
            throw new ArgumentException(
                "Need to define 3 unique inputs!"
            );
    }
}