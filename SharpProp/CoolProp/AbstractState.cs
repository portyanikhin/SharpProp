namespace SharpProp;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class AbstractState : IDisposable
{
    private static readonly object HandlesLock = new();
    private bool _disposed;
    private HandleRef _handle;

    private AbstractState(IntPtr pointer) =>
        _handle = new HandleRef(this, pointer);

    public void Dispose()
    {
        lock (HandlesLock)
        {
            if (_disposed)
                return;
        }

        InternalDispose();
        GC.SuppressFinalize(this);
    }

    ~AbstractState() => InternalDispose();

    private void InternalDispose()
    {
        lock (HandlesLock)
        {
            if (_handle.Handle == IntPtr.Zero)
                return;
            AbstractStatePInvoke.Delete(_handle);
            _handle = new HandleRef(null, IntPtr.Zero);
            _disposed = true;
        }
    }

    public static AbstractState Factory(string backend, string fluidNames)
    {
        lock (HandlesLock)
        {
            var pointer = AbstractStatePInvoke.Factory(backend, fluidNames);
            var abstractState = new AbstractState(pointer);
            SwigExceptions.ThrowPendingException();
            return abstractState;
        }
    }

    public void SetMassFractions(DoubleVector massFractions)
    {
        AbstractStatePInvoke.SetMassFractions(_handle, massFractions.Handle);
        SwigExceptions.ThrowPendingException();
    }

    public void SetVolumeFractions(DoubleVector volumeFractions)
    {
        AbstractStatePInvoke.SetVolumeFractions(
            _handle,
            volumeFractions.Handle
        );
        SwigExceptions.ThrowPendingException();
    }

    public static InputPairs? GetInputPair(string inputPairName)
    {
        try
        {
            var result = (InputPairs)
                AbstractStatePInvoke.GetInputPairIndex(inputPairName);
            SwigExceptions.ThrowPendingException();
            return result;
        }
        catch
        {
            return null;
        }
    }

    public void Update(
        InputPairs inputPair,
        double firstInput,
        double secondInput
    )
    {
        AbstractStatePInvoke.Update(
            _handle,
            (int)inputPair,
            firstInput,
            secondInput
        );
        SwigExceptions.ThrowPendingException();
    }

    public void Clear()
    {
        AbstractStatePInvoke.Clear(_handle);
        SwigExceptions.ThrowPendingException();
    }

    public void SpecifyPhase(Phases phase)
    {
        AbstractStatePInvoke.SpecifyPhase(_handle, (int)phase);
        SwigExceptions.ThrowPendingException();
    }

    public void UnspecifyPhase()
    {
        AbstractStatePInvoke.UnspecifyPhase(_handle);
        SwigExceptions.ThrowPendingException();
    }

    public double KeyedOutput(Parameters key)
    {
        var result = AbstractStatePInvoke.KeyedOutput(_handle, (int)key);
        SwigExceptions.ThrowPendingException();
        return result;
    }
}
