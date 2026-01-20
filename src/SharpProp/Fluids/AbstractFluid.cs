namespace SharpProp;

/// <inheritdoc cref="IAbstractFluid"/>
public abstract partial class AbstractFluid : IAbstractFluid
{
    private Phases? _specifiedPhase;
    protected AbstractState Backend = default!;

    protected IList<IKeyedInput<Parameters>> Inputs { get; private set; } =
        new List<IKeyedInput<Parameters>>(2);

    public void Dispose()
    {
        Backend.Dispose();
        GC.SuppressFinalize(this);
    }

    public void Update(IKeyedInput<Parameters> firstInput, IKeyedInput<Parameters> secondInput)
    {
        Reset();
        var (inputPair, firstValue, secondValue) = GenerateUpdatePair(firstInput, secondInput);
        Backend.Update(inputPair, firstValue, secondValue);
        Inputs = [firstInput, secondInput];
    }

    public virtual void Reset()
    {
        Inputs.Clear();
        Backend.Clear();
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

    protected AbstractFluid SpecifyPhase(Phases phase)
    {
        Backend.SpecifyPhase(phase);
        _specifiedPhase = phase;
        return this;
    }

    protected AbstractFluid UnspecifyPhase()
    {
        Backend.UnspecifyPhase();
        _specifiedPhase = null;
        return this;
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode() =>
        (
            string.Join("&", Inputs.Select(input => input.Value)),
            string.Join("&", Inputs.Select(input => input.CoolPropKey))
        ).GetHashCode();

    protected AbstractFluid WithState(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    )
    {
        var fluid = CreateInstance();
        if (_specifiedPhase is not null)
        {
            fluid.SpecifyPhase(_specifiedPhase.Value);
        }

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
        catch (Exception exception) when (exception is ApplicationException or ArgumentException)
        {
            return null;
        }
    }

    protected double KeyedOutput(Parameters key)
    {
        var input = Inputs.FirstOrDefault(input => input.CoolPropKey == key)?.Value;
        var result = input ?? Backend.KeyedOutput(key);
        OutputsValidator.Validate(result);
        return result;
    }

    private static UpdatePair GenerateUpdatePair(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    )
    {
        var inputPair = GetInputPair(firstInput, secondInput);
        var swap = !inputPair.HasValue;
        if (swap)
        {
            inputPair = GetInputPair(secondInput, firstInput);
        }

        if (!inputPair.HasValue)
        {
            throw new ArgumentException("Need to define 2 unique inputs!");
        }

        return !swap
            ? new UpdatePair(inputPair.Value, firstInput.Value, secondInput.Value)
            : new UpdatePair(inputPair.Value, secondInput.Value, firstInput.Value);
    }

    private static InputPairs? GetInputPair(
        IKeyedInput<Parameters> firstInput,
        IKeyedInput<Parameters> secondInput
    ) =>
        AbstractState.GetInputPair(
            $"{firstInput.CoolPropHighLevelKey}{secondInput.CoolPropHighLevelKey}_INPUTS"
        );
}
