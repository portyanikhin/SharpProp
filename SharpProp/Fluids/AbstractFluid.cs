namespace SharpProp;

/// <summary>
///     Base class of fluids.
/// </summary>
public abstract partial class AbstractFluid : IDisposable
{
    protected AbstractState Backend = null!;

    protected List<IKeyedInput<Parameters>> Inputs { get; private set; } = new(2);

    public void Dispose()
    {
        Backend.Dispose();
        GC.SuppressFinalize(this);
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

    protected virtual void Reset()
    {
        Inputs.Clear();
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

    protected AbstractFluid WithState(IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput)
    {
        var fluid = CreateInstance();
        fluid.Update(firstInput, secondInput);
        return fluid;
    }

    protected abstract AbstractFluid CreateInstance();

    protected bool KeyedOutputIsNotNull(Parameters key, out double? value)
    {
        value = NullableKeyedOutput(key);
        return value is not null;
    }

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

    protected double KeyedOutput(Parameters key)
    {
        var input = Inputs.Find(input => input.CoolPropKey == key)?.Value;
        var result = input ?? Backend.KeyedOutput(key);
        new OutputsValidator(result).Validate();
        return result;
    }

    private static UpdatePair GenerateUpdatePair(
        IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput)
    {
        var inputPair = GetInputPair(firstInput, secondInput);
        var swap = !inputPair.HasValue;
        if (swap) inputPair = GetInputPair(secondInput, firstInput);
        if (!inputPair.HasValue)
            throw new ArgumentException("Need to define 2 unique inputs!");

        return !swap
            ? new UpdatePair(inputPair.Value, firstInput.Value, secondInput.Value)
            : new UpdatePair(inputPair.Value, secondInput.Value, firstInput.Value);
    }

    private static InputPairs? GetInputPair(
        IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput) =>
        AbstractState.GetInputPair($"{firstInput.CoolPropHighLevelKey}{secondInput.CoolPropHighLevelKey}_INPUTS");

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode() =>
        (string.Join("&", Inputs.Select(input => input.Value)),
            string.Join("&", Inputs.Select(input => input.CoolPropKey)))
        .GetHashCode();
}