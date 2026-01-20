namespace SharpProp;

/// <inheritdoc cref="IHumidAir"/>
public partial class HumidAir : IHumidAir
{
    private List<IKeyedInput<string>> _inputs = new(3);

    public void Update(
        IKeyedInput<string> firstInput,
        IKeyedInput<string> secondInput,
        IKeyedInput<string> thirdInput
    )
    {
        Reset();
        _inputs = [firstInput, secondInput, thirdInput];
        CheckInputs();
    }

    public virtual void Reset()
    {
        _inputs.Clear();
        _compressibility = null;
        _conductivity = null;
        _dewTemperature = null;
        _dynamicViscosity = null;
        _enthalpy = null;
        _entropy = null;
        _humidity = null;
        _partialPressure = null;
        _pressure = null;
        _relativeHumidity = null;
        _specificHeat = null;
        _specificVolume = null;
        _temperature = null;
        _wetBulbTemperature = null;
    }

    public IHumidAir WithState(
        IKeyedInput<string> fistInput,
        IKeyedInput<string> secondInput,
        IKeyedInput<string> thirdInput
    )
    {
        var humidAir = CreateInstance();
        humidAir.Update(fistInput, secondInput, thirdInput);
        return humidAir;
    }

    public IHumidAir Clone() => WithState(_inputs[0], _inputs[1], _inputs[2]);

    public IHumidAir Factory() => CreateInstance();

    public string AsJson(bool indented = true) => this.ConvertToJson(indented);

    public bool Equals(IHumidAir? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || GetHashCode() == other.GetHashCode();
    }

    public override bool Equals(object? obj) => Equals(obj as HumidAir);

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode() =>
        (
            string.Join("&", _inputs.Select(input => input.Value)),
            string.Join("&", _inputs.Select(input => input.CoolPropKey))
        ).GetHashCode();

    protected virtual HumidAir CreateInstance() => new();

    protected double KeyedOutput(string key)
    {
        CheckInputs();
        var input = _inputs.FirstOrDefault(input => input.CoolPropKey == key)?.Value;
        var result =
            input
            ?? CoolProp.HAPropsSI(
                key,
                _inputs[0].CoolPropKey,
                _inputs[0].Value,
                _inputs[1].CoolPropKey,
                _inputs[1].Value,
                _inputs[2].CoolPropKey,
                _inputs[2].Value
            );
        OutputsValidator.Validate(result);
        return result;
    }

    private void CheckInputs()
    {
        var uniqueKeys = _inputs.Select(input => input.CoolPropKey).Distinct().ToList();
        if (_inputs.Count != 3 || uniqueKeys.Count != 3)
        {
            throw new ArgumentException("Need to define 3 unique inputs!");
        }
    }
}
