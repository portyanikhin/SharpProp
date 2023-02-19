namespace SharpProp;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class DoubleVector : IList<double>, IDisposable
{
    private static readonly object CollectionLock = new();
    private bool _disposed;

    private DoubleVector(IntPtr pointer) =>
        Handle = new HandleRef(this, pointer);

    public DoubleVector(IEnumerable<double> collection) : this(DoubleVectorPInvoke.Create())
    {
        foreach (var item in collection) Add(item);
    }

    internal HandleRef Handle { get; private set; }

    public void Dispose()
    {
        lock (CollectionLock)
        {
            if (_disposed) return;
        }

        InternalDispose();
        GC.SuppressFinalize(this);
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        new DoubleVectorEnumerator(this);

    public bool IsReadOnly => false;

    public double this[int index]
    {
        get => GetItem(index);
        set => SetItem(index, value);
    }

    public int Count => (int) DoubleVectorPInvoke.Size(Handle);

    public void CopyTo(double[] array, int arrayIndex) =>
        CopyTo(0, array, arrayIndex, Count);

    IEnumerator<double> IEnumerable<double>.
        GetEnumerator() =>
        new DoubleVectorEnumerator(this);

    public void Clear() =>
        DoubleVectorPInvoke.Clear(Handle);

    public void Add(double item) =>
        DoubleVectorPInvoke.Add(Handle, item);

    public void Insert(int index, double item)
    {
        DoubleVectorPInvoke.Insert(Handle, index, item);
        SwigExceptions.ThrowPendingException();
    }

    public void RemoveAt(int index)
    {
        DoubleVectorPInvoke.RemoveAt(Handle, index);
        SwigExceptions.ThrowPendingException();
    }

    public bool Contains(double item) =>
        DoubleVectorPInvoke.Contains(Handle, item);

    public int IndexOf(double item) =>
        DoubleVectorPInvoke.IndexOf(Handle, item);

    public bool Remove(double item) =>
        DoubleVectorPInvoke.Remove(Handle, item);

    ~DoubleVector() => InternalDispose();

    private void InternalDispose()
    {
        lock (CollectionLock)
        {
            if (Handle.Handle == IntPtr.Zero) return;
            DoubleVectorPInvoke.Delete(Handle);
            Handle = new HandleRef(null, IntPtr.Zero);
            _disposed = true;
        }
    }

    private void CopyTo(int index, double[] array, int arrayIndex, int count)
    {
        if (array is null)
            throw new ArgumentNullException(nameof(array));
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), "Value is less than zero");
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Value is less than zero");
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Value is less than zero");
        if (array.Rank > 1)
            throw new ArgumentException("Multi dimensional array.", nameof(array));
        if (index + count > Count || arrayIndex + count > array.Length)
            throw new ArgumentException("Number of elements to copy is too large.");
        for (var i = 0; i < count; i++)
            array.SetValue(GetItemCopy(index + i), arrayIndex + i);
    }

    private double GetItemCopy(int index)
    {
        var result = DoubleVectorPInvoke.GetItemCopy(Handle, index);
        SwigExceptions.ThrowPendingException();
        return result;
    }

    private double GetItem(int index)
    {
        var result = DoubleVectorPInvoke.GetItem(Handle, index);
        SwigExceptions.ThrowPendingException();
        return result;
    }

    private void SetItem(int index, double item)
    {
        DoubleVectorPInvoke.SetItem(Handle, index, item);
        SwigExceptions.ThrowPendingException();
    }

    private sealed class DoubleVectorEnumerator : IEnumerator<double>
    {
        private readonly DoubleVector _collectionRef;
        private readonly int _currentSize;
        private int _currentIndex;
        private object? _currentObject;

        public DoubleVectorEnumerator(DoubleVector collection)
        {
            _collectionRef = collection;
            _currentIndex = -1;
            _currentObject = null;
            _currentSize = _collectionRef.Count;
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            var size = _collectionRef.Count;
            var moveOkay = _currentIndex + 1 < size && size == _currentSize;
            if (moveOkay)
            {
                _currentIndex++;
                _currentObject = _collectionRef[_currentIndex];
            }
            else
            {
                _currentObject = null;
            }

            return moveOkay;
        }

        public void Reset()
        {
            _currentIndex = -1;
            _currentObject = null;
            if (_collectionRef.Count != _currentSize)
                throw new InvalidOperationException("Collection modified.");
        }

        public double Current
        {
            get
            {
                if (_currentIndex == -1)
                    throw new InvalidOperationException("Enumeration not started.");
                if (_currentIndex > _currentSize - 1)
                    throw new InvalidOperationException("Enumeration finished.");
                if (_currentObject == null)
                    throw new InvalidOperationException("Collection modified.");
                return (double) _currentObject;
            }
        }

        public void Dispose()
        {
            _currentIndex = -1;
            _currentObject = null;
        }
    }
}