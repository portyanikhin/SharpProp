namespace SharpProp;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class AbstractState : IDisposable
{
    private static readonly object HandlesLock = new();

    private AbstractState(IntPtr pointer) =>
        Handle = new HandleRef(this, pointer);

    private bool Disposed { get; set; }
    private HandleRef Handle { get; set; }

    public void Dispose()
    {
        lock (HandlesLock)
        {
            if (Disposed) return;
        }

        InternalDispose();
        GC.SuppressFinalize(this);
    }

    ~AbstractState() => InternalDispose();

    private void InternalDispose()
    {
        lock (HandlesLock)
        {
            if (Handle.Handle == IntPtr.Zero) return;
            AbstractStatePInvoke.Delete(Handle);
            Handle = new HandleRef(null, IntPtr.Zero);
            Disposed = true;
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
        AbstractStatePInvoke.SetMassFractions(Handle, massFractions.Handle);
        SwigExceptions.ThrowPendingException();
    }

    public void SetVolumeFractions(DoubleVector volumeFractions)
    {
        AbstractStatePInvoke.SetVolumeFractions(Handle, volumeFractions.Handle);
        SwigExceptions.ThrowPendingException();
    }

    public static InputPairs? GetInputPair(string inputPairName)
    {
        try
        {
            var result = (InputPairs) AbstractStatePInvoke.GetInputPairIndex(inputPairName);
            SwigExceptions.ThrowPendingException();
            return result;
        }
        catch
        {
            return null;
        }
    }

    public void Update(InputPairs inputPair, double firstInput, double secondInput)
    {
        AbstractStatePInvoke.Update(Handle, (int) inputPair, firstInput, secondInput);
        SwigExceptions.ThrowPendingException();
    }

    public double KeyedOutput(Parameters key)
    {
        var result = AbstractStatePInvoke.KeyedOutput(Handle, (int) key);
        SwigExceptions.ThrowPendingException();
        return result;
    }
}