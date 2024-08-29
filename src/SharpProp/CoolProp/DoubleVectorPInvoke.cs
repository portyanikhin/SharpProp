namespace SharpProp;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
internal static class DoubleVectorPInvoke
{
    [DllImport(Library.Name, EntryPoint = "CSharp_DoubleVector_Clear")]
    public static extern void Clear(HandleRef doubleVector);

    [DllImport(Library.Name, EntryPoint = "CSharp_DoubleVector_Add")]
    public static extern void Add(HandleRef doubleVector, double item);

    [DllImport(Library.Name, EntryPoint = "CSharp_DoubleVector_size")]
    public static extern uint Size(HandleRef doubleVector);

    [DllImport(Library.Name, EntryPoint = "CSharp_new_DoubleVector__SWIG_0")]
    public static extern IntPtr Create();

    [DllImport(Library.Name, EntryPoint = "CSharp_DoubleVector_getitemcopy")]
    public static extern double GetItemCopy(HandleRef doubleVector, int index);

    [DllImport(Library.Name, EntryPoint = "CSharp_DoubleVector_getitem")]
    public static extern double GetItem(HandleRef doubleVector, int index);

    [DllImport(Library.Name, EntryPoint = "CSharp_DoubleVector_setitem")]
    public static extern void SetItem(HandleRef arg1, int index, double item);

    [DllImport(Library.Name, EntryPoint = "CSharp_DoubleVector_Insert")]
    public static extern void Insert(HandleRef doubleVector, int index, double item);

    [DllImport(Library.Name, EntryPoint = "CSharp_DoubleVector_RemoveAt")]
    public static extern void RemoveAt(HandleRef doubleVector, int index);

    [DllImport(Library.Name, EntryPoint = "CSharp_DoubleVector_Contains")]
    public static extern bool Contains(HandleRef doubleVector, double item);

    [DllImport(Library.Name, EntryPoint = "CSharp_DoubleVector_IndexOf")]
    public static extern int IndexOf(HandleRef doubleVector, double item);

    [DllImport(Library.Name, EntryPoint = "CSharp_DoubleVector_Remove")]
    public static extern bool Remove(HandleRef doubleVector, double item);

    [DllImport(Library.Name, EntryPoint = "CSharp_delete_DoubleVector")]
    public static extern void Delete(HandleRef doubleVector);
}
