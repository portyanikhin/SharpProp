namespace SharpProp;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
internal static class AbstractStatePInvoke
{
    [DllImport(Library.Name, EntryPoint = "CSharp_delete_AbstractState")]
    public static extern void Delete(HandleRef abstractState);

    [DllImport(Library.Name, EntryPoint = "CSharp_AbstractState_factory__SWIG_0")]
    public static extern IntPtr Factory(string backend, string fluidNames);

    [DllImport(Library.Name, EntryPoint = "CSharp_AbstractState_set_mass_fractions")]
    public static extern void SetMassFractions(HandleRef abstractState, HandleRef massFractions);

    [DllImport(Library.Name, EntryPoint = "CSharp_AbstractState_set_volu_fractions")]
    public static extern void SetVolumeFractions(HandleRef abstractState, HandleRef volumeFractions);

    [DllImport(Library.Name, EntryPoint = "CSharp_get_input_pair_index")]
    public static extern int GetInputPairIndex(string inputPairName);

    [DllImport(Library.Name, EntryPoint = "CSharp_AbstractState_update")]
    public static extern void Update(HandleRef abstractState, int inputPair, double firstInput, double secondInput);

    [DllImport(Library.Name, EntryPoint = "CSharp_AbstractState_keyed_output")]
    public static extern double KeyedOutput(HandleRef abstractState, int key);
}